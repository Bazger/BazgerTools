using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.Converters;
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

        private ManualResetEvent _stoppedEvent;
        private AutoResetEvent _stageEvent;

        private readonly IWebSiteDownloaderProxy _websiteProxy;
        private readonly IAudioConverterProxy _converterProxy;

        public Launcher(IEnumerable<string> videoUrls, DownloaderConfigs configs = null, string name = "Launcher") : base(name)
        {
            _configs = configs ?? DownloaderConfigs.GetDefaultConfigs();
            _videoUrls = videoUrls;

            _websiteProxy = new YouTubeProxy();
            _converterProxy = new FFmpegConverterProxy();

            VideosProgress = new ConcurrentDictionary<string, VideoProgressMetadata>();
            _downloaderThreads = new List<DownloaderThread>();
            _converterThreads = new List<ConverterThread>();
        }

        public override void Start()
        {
            StopEvent = new ManualResetEvent(false);
            _stoppedEvent = new ManualResetEvent(false);

            JobThread.Start();
            IsStarted = true;
        }

        public override void Stop()
        {
            //TODO: Create smatrter stop
            StopEvent.Set();
            while (JobThread.IsAlive)
            {
                if (!_stoppedEvent.WaitOne(10000))
                {
                    Log.Warn("Abort worker thread");

                    JobThread.Abort();
                }
            }
        }

        protected override void Job()
        {
            //TODO: Clear old downloaders converters and videometada
            var waitingForDownload = new BlockingCollection<string>(new ConcurrentQueue<string>(_videoUrls));
            var waitingForConvertion = new BlockingCollection<VideoProperties>();
            _stageEvent = new AutoResetEvent(false);

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

            //TODO: CREATE AUTO EVENT FOR BEST OUTPUT CAPTURING
            Processing();

            if (!_configs.WriteToJournal) return;
            Log.Info("Writing to journal");
            WriteToJournal();
            Log.Info("Writing succeeded");
        }

        private void StartDownloderThreads(BlockingCollection<string> waitingForDownload, BlockingCollection<VideoProperties> waitingForConvertion)
        {

            for (var i = 0; i < _configs.ParallelDownloadsCount; i++)
            {
                _downloaderThreads.Add(new DownloaderThread($"Downloader {i}", _websiteProxy, VideosProgress, _configs.SaveDir, waitingForDownload, waitingForConvertion, _configs.ConverterEnabled));
            }

            Log.Info("Starting downloader threads");
            while (!_stageEvent.WaitOne(200))
            {
                foreach (var service in _downloaderThreads.Where(c => !c.IsAlive && c.IsEnabled))
                    try
                    {
                        Log.Info("Starting service ({0})", service.Name);
                        service.Start();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error starting service ({0}): {1}", service.Name, ex);
                    }
                _stageEvent.Set();
            }
            Log.Info("{0} downloading threads was started", _configs.ParallelDownloadsCount);
        }

        private void StartConverterThreads(BlockingCollection<VideoProperties> waitingForConvertion)
        {
            for (var i = 0; i < _configs.ConvertersCount; i++)
            {
                _converterThreads.Add(new ConverterThread($"Converter {i}", _converterProxy, waitingForConvertion,
                    VideosProgress, _configs.ConvertionFormat, _downloaderThreads));
            }

            Log.Info("Starting converterProxy threads");
            while (!_stageEvent.WaitOne(200))
            {
                foreach (var service in _converterThreads.Where(c => !c.IsAlive && c.IsEnabled))
                    try
                    {
                        Log.Info("Starting service ({0})", service.Name);
                        service.Start();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error starting service ({0}): {1}", service.Name, ex);
                    }
                _stageEvent.Set();
            }
        }

        private void Processing()
        {
            while (!_stageEvent.WaitOne(1000))
            {
                //Check if threads downloading and converting
                if (_downloaderThreads.Count(c => c.IsAlive) != 0 || _converterThreads.Count(c => c.IsAlive) != 0)
                {
                    continue;
                }
                _stageEvent.Set();
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
            catch (Exception)
            {
                Log.Error("there is a problem to write to journal file. May be journal format is illegal");
            }
        }

        
        private  IEnumerable<string> ReadFromJournal()
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
            catch (Exception)
            {
                Log.Error("There is a problem to write to journal file. May be journal format is illegal");
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
