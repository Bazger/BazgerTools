using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using Bazger.Tools.YouTubeDownloader.Core.WebSites;
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

        //TODO: Add option to chose video quality and resolution
        public Launcher(IEnumerable<string> videoUrls, DownloaderConfigs configs, string name = "Launcher") : base(name)
        {
            _configs = configs;
            _videoUrls = videoUrls;

            VideosProgress = new ConcurrentDictionary<string, VideoProgressMetadata>();
            _downloaderThreads = new List<DownloaderThread>();
            _converterThreads = new List<ConverterThread>();
        }

        public override void Start()
        {
            StopEvent = new ManualResetEvent(false);

            ClearData();
            JobThread.Start();
            IsStarted = true;
        }

        public override void Stop()
        {
            if (StopEvent.WaitOne(0))
            {
                Log.Info("Launcher service already stopped");
            }
            Log.Info("Stopping launcher service");
            StopEvent.Set();
            StopDownloaderThreads();
            if (_configs.ConverterEnabled)
            {
                StopConvertersThreads();
            }

            var waitEvent = new ManualResetEvent(false);
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

                if (JobThread.IsAlive)
                {
                    Log.Warn("Abort launcher thread");
                    JobThread.Abort();
                }
            }

            Log.Info("Removing temporary files");
            RemoveTemporaryFiles();

            if (!_configs.WriteToJournal)
            {
                return;
            }
            Log.Info("Writing to journal");
            WriteToJournal();
            Log.Info("Writing succeeded");

            Log.Info("Launcher stopped");
        }

        private void RemoveTemporaryFiles()
        {
            foreach (var videoMetadata in VideosProgress.Values.Where(v => v.Stage != VideoProgressStage.WaitingToConvertion))
            {
                //Remove video file
                if (File.Exists(videoMetadata.VideoFilePath))
                {
                    File.Delete(videoMetadata.VideoFilePath);
                }
            }
        }

        protected override void Job()
        {
            var waitingForDownload = new BlockingCollection<string>(new ConcurrentQueue<string>(_videoUrls));
            var waitingForConvertion = new BlockingCollection<VideoProgressMetadata>();

            //STAGE I - Initializing
            if (StopEvent.WaitOne(0))
            {
                return;
            }
            Directory.CreateDirectory(_configs.SaveDir);

            if (_configs.ReadFromJournal)
            {
                Log.Info("Reading from journal");
                var downloadedVideos = ReadFromJournal();
                //Get the dif from downloaded videos and all videos
                if (downloadedVideos != null)
                {
                    _videoUrls = _videoUrls.Where(url => !downloadedVideos.Contains(url));
                }
            }

            StartDownloderThreads(waitingForDownload, waitingForConvertion);
            if (_configs.ConverterEnabled)
            {
                StartConverterThreads(waitingForConvertion);
            }

            //STAGE II - Processing
            //Check if threads downloading and converting
            while (_downloaderThreads.Count(c => c.IsAlive) != 0 || _converterThreads.Count(c => c.IsAlive) != 0)
            {
                //Check if stop of thread called
                if (StopEvent.WaitOne(1000))
                {
                    return;
                }
            }

            //STAGE III - Finishing
            if (StopEvent.WaitOne(1000))
            {
                return;
            }
            if (!_configs.WriteToJournal)
            {
                return;
            }
            Log.Info("Writing to journal");
            WriteToJournal();
            Log.Info("Writing succeeded");
        }

        public override void Abort()
        {
            Log.Warn($"Abort launcher service ({Name})");
            JobThread.Abort();
        }

        private void ClearData()
        {
            VideosProgress.Clear();
            _downloaderThreads.Clear();
            _converterThreads.Clear();
        }

        private void StartDownloderThreads(BlockingCollection<string> waitingForDownload, BlockingCollection<VideoProgressMetadata> waitingForConvertion)
        {
            //Inialing once because method are safe for multithreaded usage
            for (var i = 0; i < _configs.ParallelDownloadsCount; i++)
            {
                _downloaderThreads.Add(new DownloaderThread($"Downloader {i}", VideosProgress, _configs.SaveDir, waitingForDownload, waitingForConvertion, _configs.ConverterEnabled));
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
            foreach (var service in _downloaderThreads.Reverse<DownloaderThread>().Where(c => c.IsStarted && c.IsAlive))
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
            foreach (var service in _downloaderThreads.Reverse<DownloaderThread>().Where(c => c.IsStarted && c.IsAlive))
            {
                service.Abort();
            }
        }

        private void StartConverterThreads(BlockingCollection<VideoProgressMetadata> waitingForConvertion)
        {
            for (var i = 0; i < _configs.ConvertersCount; i++)
            {
                _converterThreads.Add(new ConverterThread($"Converter {i}", waitingForConvertion, _configs.ConvertionFormat, _downloaderThreads));
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
            foreach (var service in _converterThreads.Reverse<ConverterThread>().Where(c => c.IsStarted && c.IsAlive))
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
            foreach (var service in _converterThreads.Reverse<ConverterThread>().Where(c => c.IsStarted && c.IsAlive))
            {
                service.Abort();
            }
        }

        private void WriteToJournal()
        {
            try
            {
                if (!File.Exists(_configs.JournalFileName))
                {
                    Log.Warn("Journal file doen't exist. Creating new one");
                    SerDeUtils.SerializeToJsonFile(new HashSet<string>(), _configs.JournalFileName);
                }
                var downloadedVideos = SerDeUtils.DeserializeJsonFile<HashSet<string>>(_configs.JournalFileName);
                foreach (
                    var videoUrl in
                        _videoUrls.Where(
                            url =>
                                VideosProgress.ContainsKey(url) &&
                                VideosProgress[url].Stage == VideoProgressStage.Completed))
                {
                    downloadedVideos.Add(videoUrl);
                }
                SerDeUtils.SerializeToJsonFile(downloadedVideos, _configs.JournalFileName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "There is a problem to write to a journal file");
            }
        }


        private IEnumerable<string> ReadFromJournal()
        {
            if (!File.Exists(_configs.JournalFileName))
            {
                Log.Warn("Journal file doen't exist");
                return null;
            }
            try
            {
                return SerDeUtils.DeserializeJsonFile<HashSet<string>>(_configs.JournalFileName);
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
    }
}