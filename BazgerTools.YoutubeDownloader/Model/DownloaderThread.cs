using System;
using System.Collections.Concurrent;
using System.Threading;
using BazgerTools.YouTubeDownloader.WebSites;
using NLog;

namespace BazgerTools.YouTubeDownloader.Model
{
    public class DownloaderThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly BlockingCollection<string> _waitingForDownload;
        private readonly BlockingCollection<Tuple<string, string>> _waitingToConvert;
        private readonly bool _isConvertionEnabled;
        private readonly IWebSiteDownloader _webSite;
        private readonly ConcurrentDictionary<string, VideoProgressMetadata> _videosProgress;
        private readonly string _saveDir;
        private ManualResetEvent _stoppedEvent;
        private readonly int _millisecondsTimeout = 5000;

        public DownloaderThread(string name, IWebSiteDownloader webSite, ConcurrentDictionary<string, VideoProgressMetadata> videosProgress, string saveDir, BlockingCollection<string> waitingForDownload, BlockingCollection<Tuple<string, string>> waitingToConvert, bool isConvertionEnabled) : base(name)
        {
            _waitingForDownload = waitingForDownload;
            _waitingToConvert = waitingToConvert;
            _isConvertionEnabled = isConvertionEnabled;
            _webSite = webSite;
            _videosProgress = videosProgress;
            _saveDir = saveDir;
        }


        protected override void Job()
        {
            while (_waitingForDownload.Count != 0 && !StopEvent.WaitOne(0))
            {
                string videoUrl = null;
                try
                {
                    _waitingForDownload.TryTake(out videoUrl, _millisecondsTimeout);
                    if (videoUrl == null)
                    {
                        continue;
                    }
                    _videosProgress.TryAdd(videoUrl,
                        new VideoProgressMetadata() { Stage = VideoProgressStage.Downloading, Progress = 0 });
                    Log.Info($"Downloading video | url={videoUrl}");
                    var videoPath = _webSite.Download(videoUrl, _saveDir, _videosProgress[videoUrl]);
                    if (!_isConvertionEnabled)
                    {
                        _videosProgress[videoUrl].Stage = VideoProgressStage.Completed;
                        continue;
                    }
                    _waitingToConvert?.TryAdd(new Tuple<string, string>(videoUrl, videoPath));
                    _videosProgress[videoUrl].Stage = VideoProgressStage.WaitingToConvertion;
                }
                catch (Exception ex)
                {
                    if (videoUrl == null)
                    {
                        continue;
                    }
                    Log.Error($"Can't download video | url={videoUrl} | Retries={_videosProgress[videoUrl].Retries} \n" + ex);
                    _videosProgress[videoUrl].Stage = VideoProgressStage.Error;
                    _videosProgress[videoUrl].ErrorArgs = ex.ToString();
                }
            }
            _stoppedEvent.Set();
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
    }
}
