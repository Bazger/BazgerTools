using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using Bazger.Tools.YouTubeDownloader.Core.WebSites;
using ConcurrentCollections;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core
{
    public class Launcher : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ConcurrentDictionary<string, VideoProgressMetadata> VideosProgress { get; }

        private readonly DownloaderConfigs _configs;
        private IEnumerable<string> _videoUrls;

        private readonly List<DownloaderThread> _downloaderThreads;
        private readonly List<ConverterThread> _converterThreads;
        private FileMoverThread _fileMoverThread;

        private string _tempDir;

        //TODO: Add option to chose video quality and resolution
        public Launcher(IEnumerable<string> videoUrls, DownloaderConfigs configs, string name = "Launcher") : base(name)
        {
            _configs = configs;
            _videoUrls = videoUrls;

            VideosProgress = new ConcurrentDictionary<string, VideoProgressMetadata>();
            _downloaderThreads = new List<DownloaderThread>();
            _converterThreads = new List<ConverterThread>();
        }

        public override void Stop()
        {
            if (StoppedEvent.WaitOne(0))
            {
                Log.Info("Launcher service already stopped");
                return;
            }
            if (StopEvent.WaitOne(0))
            {
                Log.Info("Launcher service is stopping now");
                return;
            }

            Log.Info("Stopping launcher service");
            StopEvent.Set();
            StopDownloaderThreads();
            if (_configs.ConverterEnabled)
            {
                StopConvertersThreads();
            }

            var waitEvent = new AutoResetEvent(false);
            while (!waitEvent.WaitOne(5000))
            {
                if (!JobThread.IsAlive && _downloaderThreads.Count(c => c.IsAlive) == 0 &&
                    _converterThreads.Count(c => c.IsAlive) == 0)
                {
                    waitEvent.Set();
                    break;
                }
                AbortDownloaderThreads();
                AbortConvertersThreads();
                this.Abort();
            }

            Log.Info("Stopping File Mover service");
            _fileMoverThread.Stop();
            while (_fileMoverThread.IsAlive && !waitEvent.WaitOne(1000))
            {
                _fileMoverThread.Abort();
            }

            Log.Info("Removing temporary files");
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }

            if (!_configs.WriteToJournal)
            {
                return;
            }
            Log.Info("Writing to journal");
            WriteToJournal();
            Log.Info("Writing succeeded");

            StoppedEvent.Set();
            Log.Info("Launcher stopped");
        }

        protected override void Job()
        {
            VideosProgress.Clear();
            _downloaderThreads.Clear();
            _converterThreads.Clear();

            var waitingForDownload = new BlockingCollection<string>(new ConcurrentQueue<string>(_videoUrls));
            var waitingForConvertion = new BlockingCollection<VideoProgressMetadata>();
            var waitingForMoving = new BlockingCollection<VideoProgressMetadata>();

            //STAGE I - Initializing
            if (StopEvent.WaitOne(0))
            {
                return;
            }
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
            Directory.CreateDirectory(_configs.SaveDir);

            if (_configs.ReadFromJournal)
            {
                Log.Info("Reading from journal");
                var downloadedVideos = ReadFromJournal();
                if (downloadedVideos != null)
                {
                    //Get the dif from downloaded videos and all videos
                    _videoUrls = _videoUrls.Except(downloadedVideos);
                    Log.Info("Journal file loaded successfully");
                }

            }

            StartDownloderThreads(waitingForDownload, waitingForConvertion, waitingForMoving);
            if (_configs.ConverterEnabled)
            {
                StartConverterThreads(waitingForConvertion, waitingForMoving);
            }
            _fileMoverThread = new FileMoverThread("File Mover", waitingForMoving, _configs.OverwriteEnabled);
            _fileMoverThread.Start();

            //STAGE II - Processing
            //Check if threads downloading and converting
            while (_downloaderThreads.Count(c => c.IsAlive) != 0 || _converterThreads.Count(c => c.IsAlive) != 0 || _fileMoverThread.IsAlive)
            {
                //Check if stop of thread called
                if (StopEvent.WaitOne(1000))
                {
                    return;
                }
                if (_downloaderThreads.Count(c => c.IsAlive) == 0 && waitingForConvertion.Count == 0 && waitingForDownload.Count == 0)
                {
                    StopConvertersThreads();
                    if (_converterThreads.Count(c => c.IsAlive) == 0 && waitingForMoving.Count == 0)
                    {
                        _fileMoverThread.Stop();
                    }
                }
            }

            //STAGE III - Finishing
            if (StopEvent.WaitOne(1000))
            {
                return;
            }

            Log.Info("Removing temporary files");
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
            if (!_configs.WriteToJournal)
            {
                return;
            }
            Log.Info("Writing to journal");
            WriteToJournal();
            Log.Info("Writing succeeded");

            StoppedEvent.Set();
            Log.Info("Launcher finished its work");
        }

        public override void Abort()
        {
            if (!JobThread.IsAlive)
            {
                return;
            }
            Log.Warn($"Abort launcher service ({Name})");
            JobThread.Abort();
        }

        private void StartDownloderThreads(BlockingCollection<string> waitingForDownload, BlockingCollection<VideoProgressMetadata> waitingForConvertion, BlockingCollection<VideoProgressMetadata> waitingForMoving)
        {
            //Inialing once because method are safe for multithreaded usage
            for (var i = 0; i < _configs.ParallelDownloadsCount; i++)
            {
                _downloaderThreads.Add(new DownloaderThread($"Downloader {i}", VideosProgress, _configs.SaveDir, _tempDir, waitingForDownload, waitingForConvertion, waitingForMoving, _configs.ConverterEnabled));
            }

            Log.Info("Starting downloader threads");
            foreach (var service in _downloaderThreads.Where(c => !c.IsAlive && c.IsEnabled))
            {
                try
                {
                    Log.Info("Starting service ({0})", service.Name);
                    service.Start();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error starting service ({0})", service.Name);
                }
            }
            Log.Info("{0} downloading threads was started", _downloaderThreads.Count(c => c.IsStarted));
        }

        private void StopDownloaderThreads()
        {
            foreach (var service in _downloaderThreads.Reverse<DownloaderThread>())
            {
                try
                {
                    Log.Info("Stopping service ({0})", service.Name);
                    service.Stop();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error stopping service ({0})", service.Name);
                }
            }
        }

        private void AbortDownloaderThreads()
        {
            foreach (var service in _downloaderThreads.Reverse<DownloaderThread>())
            {
                service.Abort();
            }
        }

        private void StartConverterThreads(BlockingCollection<VideoProgressMetadata> waitingForConvertion, BlockingCollection<VideoProgressMetadata> waitingForMoving)
        {
            for (var i = 0; i < _configs.ConvertersCount; i++)
            {
                _converterThreads.Add(new ConverterThread($"Converter {i}", waitingForConvertion, waitingForMoving, _configs.ConvertionFormat, _tempDir));
            }

            Log.Info("Starting converter threads");
            foreach (var service in _converterThreads.Where(c => !c.IsAlive && c.IsEnabled))
            {
                try
                {
                    Log.Info("Starting service ({0})", service.Name);
                    service.Start();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error starting service ({0})", service.Name);
                }
            }
            Log.Info("{0} converter threads was started", _converterThreads.Count(c => c.IsStarted));
        }

        private void StopConvertersThreads()
        {
            foreach (var service in _converterThreads.Reverse<ConverterThread>())
            {
                try
                {
                    Log.Info("Stopping service ({0})", service.Name);
                    service.Stop();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error stopping service ({0})", service.Name);
                }
            }
        }

        private void AbortConvertersThreads()
        {
            foreach (var service in _converterThreads.Reverse<ConverterThread>())
            {
                service.Abort();
            }
        }

        private void WriteToJournal()
        {
            try
            {
                if (!File.Exists(_configs.JournalFilePath))
                {
                    Log.Warn("Journal file doen't exist. Creating new one");
                    SerDeHelper.SerializeToJsonFile(new HashSet<string>(), _configs.JournalFilePath);
                }
                var downloadedVideos = SerDeHelper.DeserializeJsonFile<HashSet<string>>(_configs.JournalFilePath);
                foreach (
                    var videoUrl in
                        _videoUrls.Where(
                            url =>
                                VideosProgress.ContainsKey(url) &&
                                VideosProgress[url].Stage == VideoProgressStage.Completed))
                {
                    downloadedVideos.Add(videoUrl);
                }
                SerDeHelper.SerializeToJsonFile(downloadedVideos, _configs.JournalFilePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "There is a problem to write to a journal file");
            }
        }


        private IEnumerable<string> ReadFromJournal()
        {
            if (!File.Exists(_configs.JournalFilePath))
            {
                Log.Warn($"Journal file doen't exist | path={_configs.JournalFilePath}");
                return null;
            }
            else
            {
                Log.Info($"Journal file was found | path={_configs.JournalFilePath}");
            }

            try
            {
                return SerDeHelper.DeserializeJsonFile<HashSet<string>>(_configs.JournalFilePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "There is a problem to read a journal file. May be journal format is illegal");
            }
            return null;
        }


        public int GetAllDownloadersCount()
        {
            return _downloaderThreads.Count;
        }

        public int GetAliveDownloadersCount()
        {
            return _downloaderThreads.Count(c => c.IsAlive);
        }

        public int GetAllConvertersCount()
        {
            return _converterThreads.Count;
        }

        public int GetAliveConvertersCount()
        {
            return _converterThreads.Count(c => c.IsAlive);
        }

        public int GetAllFileMoversCount()
        {
            return 1;
        }

        public int GetAliveFileMoversCount()
        {
            return _fileMoverThread.IsAlive ? 1 : 0;
        }
    }
}