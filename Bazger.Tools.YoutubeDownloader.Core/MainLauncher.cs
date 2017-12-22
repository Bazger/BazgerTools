using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core
{
    public class MainLauncher : LauncherBase
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
        public MainLauncher(IEnumerable<string> videoUrls, DownloaderConfigs configs, string name = "MainLauncher") : base(name)
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
            StopServices(_downloaderThreads);
            if (_configs.ConverterEnabled)
            {
                StopServices(_converterThreads);
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
                AbortServices(_downloaderThreads);
                AbortServices(_converterThreads);
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

            if (_configs.WriteToJournal)
            {
                Log.Info("Writing to journal");
                WriteToJournal();
                Log.Info("Writing succeeded");
            }

            StoppedEvent.Set();
            Log.Info("Launcher stopped");
        }

        protected override void Job()
        {
            VideosProgress.Clear();
            _downloaderThreads.Clear();
            _converterThreads.Clear();

            //STAGE I - Initializing
            if (StopEvent.WaitOne(0))
            {
                return;
            }
            _tempDir = Path.Combine(Path.GetTempPath(), "YouTubeDownloader-{" + Guid.NewGuid() + "}");
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

            StartupVidesoProgress();

            var waitingForDownload = new BlockingCollection<VideoProgressMetadata>(new ConcurrentQueue<VideoProgressMetadata>(_videoUrls.Select(url => VideosProgress[url])));
            var waitingForConvertion = new BlockingCollection<VideoProgressMetadata>();
            var waitingForMoving = new BlockingCollection<VideoProgressMetadata>();

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
                    StopServices(_converterThreads);
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
            if (_configs.WriteToJournal)
            {
                Log.Info("Writing to journal");
                WriteToJournal();
                Log.Info("Writing succeeded");
            }

            StoppedEvent.Set();
            Log.Info("Launcher finished its work");
        }

        private void StartupVidesoProgress()
        {
            foreach (var url in _videoUrls)
            {
                var progressMetadata = new VideoProgressMetadata(url)
                {
                    SaveDir = _configs.SaveDir,
                    Stage = VideoProgressStage.Idling,
                    Progress = 0
                };
                VideosProgress.TryAdd(url, progressMetadata);
            }
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

        public void ForceStop()
        {
            if (!JobThread.IsAlive)
            {
                return;
            }
            Log.Warn($"Abort launcher service ({Name})");
            AbortServices(_downloaderThreads);
            AbortServices(_converterThreads);
            _fileMoverThread.Abort();
            Log.Info("Removing temporary files");
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }

        private void StartDownloderThreads(BlockingCollection<VideoProgressMetadata> waitingForDownload, BlockingCollection<VideoProgressMetadata> waitingForConvertion, BlockingCollection<VideoProgressMetadata> waitingForMoving)
        {
            //Inialing once because method are safe for multithreaded usage
            for (var i = 0; i < _configs.ParallelDownloadsCount; i++)
            {
                _downloaderThreads.Add(new DownloaderThread($"Downloader {i}", waitingForDownload, waitingForConvertion, waitingForMoving, _tempDir, _configs.ConverterEnabled));
            }

            Log.Info("Starting downloader threads");
            StartServices(_downloaderThreads);
            Log.Info("Downloader threads was started");
        }

        private void StartConverterThreads(BlockingCollection<VideoProgressMetadata> waitingForConvertion, BlockingCollection<VideoProgressMetadata> waitingForMoving)
        {
            for (var i = 0; i < _configs.ConvertersCount; i++)
            {
                _converterThreads.Add(new ConverterThread($"Converter {i}", waitingForConvertion, waitingForMoving, _configs.ConvertionFormat, _tempDir));
            }

            Log.Info("Starting converter threads");
            StartServices(_converterThreads);
            Log.Info("Converter threads was started");
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