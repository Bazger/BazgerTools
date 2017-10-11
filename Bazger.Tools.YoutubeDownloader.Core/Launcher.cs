using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
            _stoppedEvent = new ManualResetEvent(false);

            ClearData();
            JobThread.Start();
            IsStarted = true;
        }

        public override void Stop()
        {
            StopEvent.Set();
            while (JobThread.IsAlive)
            {
                if (!_stoppedEvent.WaitOne(5000))
                {
                    Log.Warn("Abort launcher thread");
                    JobThread.Abort();
                }
            }
            StopDownloaderThreads();
            if (_configs.ConverterEnabled)
            {
                StopConvertersThreads();
            }
            if (!_configs.WriteToJournal)
            {
                return;
            }
            Log.Info("Writing to journal");
            WriteToJournal();
            Log.Info("Writing succeeded");
        }

        protected override void Job()
        {
            //TODO: Log fix (Unnensesary logs dont show on the console)
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

            _stoppedEvent.Set();
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
            var websiteProxy = new YouTubeProxy();
            for (var i = 0; i < _configs.ParallelDownloadsCount; i++)
            {
                _downloaderThreads.Add(new DownloaderThread($"Downloader {i}", websiteProxy, VideosProgress, _configs.SaveDir, waitingForDownload, waitingForConvertion, _configs.ConverterEnabled));
            }

            Log.Info("Starting downloader threads");
            foreach (var service in _downloaderThreads.Where(c => !c.IsAlive && c.IsEnabled))
            {
                try
                {
                    Log.Info("Starting service [{0}]", service.Name);
                    service.Start();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error starting service [{0}]", service.Name);
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
                    Log.Info("Stopping service [{0}]", service.Name);
                    service.Stop();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error stopping service [{0}]", service.Name);
                }
            }
        }

        private void StartConverterThreads(BlockingCollection<VideoProgressMetadata> waitingForConvertion)
        {
            for (var i = 0; i < _configs.ConvertersCount; i++)
            {
                //TODO: may be architecture changes needed
                //Initialize new proxy for all converter service because proxy includes monitoring of external process and it terminating
                _converterThreads.Add(new ConverterThread($"Converter {i}", new FFmpegConverterProxy(), waitingForConvertion, _configs.ConvertionFormat, _downloaderThreads));
            }

            Log.Info("Starting converter threads");
            foreach (var service in _converterThreads.Where(c => !c.IsAlive && c.IsEnabled))
            {
                try
                {
                    Log.Info("Starting service [{0}]", service.Name);
                    service.Start();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error starting service [{0}]", service.Name);
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
                    Log.Info("Stopping service [{0}]", service.Name);
                    service.Stop();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error stopping service [{0}]", service.Name);
                }
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
