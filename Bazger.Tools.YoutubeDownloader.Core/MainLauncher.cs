using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core
{
    public class MainLauncher : LauncherBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly DownloaderConfigs _configs;
        private readonly List<string> _videoUrls;
        private readonly IDictionary<string, VideoProgressMetadata> _inputVideosProgress;

        private readonly List<DownloaderThread> _downloaderThreads;
        private readonly List<ConverterThread> _converterThreads;
        private FileMoverThread _fileMoverThread;

        private string _tempDir;
        private readonly bool _isAfterPreview;

        private readonly Mutex _deleteDirMutex;

        public MainLauncher(IDictionary<string, VideoProgressMetadata> videosProgress, DownloaderConfigs configs, IEnumerable<string> videoUrls = null, string name = "MainLauncher") :
            this(configs, name)
        {
            _inputVideosProgress = videosProgress ?? new Dictionary<string, VideoProgressMetadata>();
            _videoUrls = videoUrls?.ToList() ?? _inputVideosProgress.Keys.ToList();
            _isAfterPreview = true;
        }

        public MainLauncher(IEnumerable<string> videoUrls, DownloaderConfigs configs, string name = "MainLauncher") :
            this(configs, name)
        {
            _videoUrls = videoUrls.ToList();
        }

        private MainLauncher(DownloaderConfigs configs, string name) : base(name)
        {
            _configs = configs;
            _downloaderThreads = new List<DownloaderThread>();
            _converterThreads = new List<ConverterThread>();
            _isAfterPreview = false;
            _deleteDirMutex = new Mutex();
        }

        public override void Stop()
        {
            if (StoppedEvent.WaitOne(0))
            {
                Log.Info($"{Name} service already stopped");
                return;
            }
            if (StopEvent.WaitOne(0))
            {
                Log.Info($"{Name} service is stopping now");
                return;
            }

            Log.Info($"Stopping {Name} service");
            StopEvent.Set();
            StopServices(_downloaderThreads);
            if (_configs.ConverterEnabled)
            {
                StopServices(_converterThreads);
            }

            var waitEvent = new AutoResetEvent(false);
            while (!waitEvent.WaitOne(1500))
            {
                if (!JobThread.IsAlive && GetAliveDownloadersCount() == 0 && GetAliveConvertersCount() == 0)
                {
                    waitEvent.Set();
                    break;
                }
                AbortServices(_downloaderThreads);
                AbortServices(_converterThreads);
            }

            Log.Info("Stopping File Mover service");
            _fileMoverThread.Stop();
            while (_fileMoverThread.IsAlive && !waitEvent.WaitOne(1000))
            {
                _fileMoverThread.Abort();
            }

            while ((GetAliveDownloadersCount() != 0 || GetAliveConvertersCount() != 0 || GetAliveFileMoversCount() != 0) &&
                !StoppedEvent.WaitOne(200))
            {
            }

            Log.Info("Removing temporary files");
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }

            StoppedEvent.Set();
            Log.Info($"{Name} service stopped");
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

            StartupVideosProgress();
            BlockingCollection<VideoProgressMetadata> waitingForDownload;
            if (_videoUrls != null && _videoUrls.Any())
            {
                waitingForDownload = new BlockingCollection<VideoProgressMetadata>(
                    new ConcurrentQueue<VideoProgressMetadata>(_videoUrls.Select(url => VideosProgress[url])));
            }
            else
            {
                waitingForDownload = new BlockingCollection<VideoProgressMetadata>(new ConcurrentQueue<VideoProgressMetadata>(VideosProgress.Values));
            }
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
            while (GetAliveDownloadersCount() != 0 || GetAliveConvertersCount() != 0 || GetAliveFileMoversCount() != 0)
            {
                //Check if stop of thread called
                if (StopEvent.WaitOne(1000))
                {
                    return;
                }
                if (GetAliveDownloadersCount() == 0 && waitingForConvertion.Count == 0 && waitingForDownload.Count == 0)
                {
                    StopServices(_converterThreads);
                    if (GetAliveConvertersCount() == 0 && waitingForMoving.Count == 0)
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
            DeleteTempDirectory();


            Log.Info($"{Name} finished its work");
            base.Job();
        }

        private void StartupVideosProgress()
        {
            foreach (var url in _videoUrls)
            {
                var progressMetadata = new VideoProgressMetadata(url)
                {
                    SaveDir = _configs.SaveDir,
                    Progress = 0
                };
                if (_isAfterPreview)
                {
                    progressMetadata.SelectedVideoType = _inputVideosProgress[url].SelectedVideoType;
                    progressMetadata.Title = _inputVideosProgress[url].Title;
                }
                VideosProgress.TryAdd(url, progressMetadata);
            }
        }

        public override void Abort()
        {
            if (StoppedEvent.WaitOne(0))
            {
                return;
            }
            Log.Warn($"Abort launcher service ({Name})");
            StopEvent.Set();
            AbortServices(_downloaderThreads);
            AbortServices(_converterThreads);
            _fileMoverThread.Abort();
            while ((GetAliveDownloadersCount() != 0 || GetAliveConvertersCount() != 0 || GetAliveFileMoversCount() != 0) && !StoppedEvent.WaitOne(100))
            {
            }
            Log.Info("Removing temporary files");
            DeleteTempDirectory();
            StoppedEvent.Set();
        }

        private void DeleteTempDirectory()
        {
            _deleteDirMutex.WaitOne();
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
            _deleteDirMutex.ReleaseMutex();
        }

        private void StartDownloderThreads(BlockingCollection<VideoProgressMetadata> waitingForDownload, BlockingCollection<VideoProgressMetadata> waitingForConvertion, BlockingCollection<VideoProgressMetadata> waitingForMoving)
        {
            //Inialing once because method are safe for multithreaded usage
            var downloadersCount = waitingForDownload.Count <= _configs.ParallelDownloadsCount
                ? waitingForDownload.Count
                : _configs.ParallelDownloadsCount;
            for (var i = 0; i < downloadersCount; i++)
            {
                _downloaderThreads.Add(new DownloaderThread($"Downloader {i}",
                    waitingForDownload, waitingForConvertion, waitingForMoving,
                    _tempDir, _configs.ConverterEnabled, _isAfterPreview,
                    VideoType.AvailabledVideoTypesDictionary[_configs.YouTubeVideoTypeId]));
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