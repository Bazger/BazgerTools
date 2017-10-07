using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.WebSites;
using NLog;
using YoutubeExtractor;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class DownloaderThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly BlockingCollection<string> _waitingForDownload;
        private readonly BlockingCollection<VideoProperties> _waitingToConvert;
        private readonly bool _isConvertionEnabled;
        private readonly IWebSiteDownloaderProxy _webSite;
        private readonly ConcurrentDictionary<string, VideoProgressMetadata> _videosProgress;
        private readonly string _saveDir;
        private ManualResetEvent _stoppedEvent;
        private readonly int _millisecondsTimeout = 5000;

        public DownloaderThread(string name, IWebSiteDownloaderProxy webSite, ConcurrentDictionary<string, VideoProgressMetadata> videosProgress, string saveDir, BlockingCollection<string> waitingForDownload, BlockingCollection<VideoProperties> waitingToConvert, bool isConvertionEnabled) : base(name)
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
                var videoUrl = string.Empty;
                try
                {
                    _waitingForDownload.TryTake(out videoUrl, _millisecondsTimeout);
                    if (string.IsNullOrEmpty(videoUrl))
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
                    _waitingToConvert?.TryAdd(new VideoProperties { Url = videoUrl, Path = videoPath });
                    _videosProgress[videoUrl].Stage = VideoProgressStage.WaitingToConvertion;
                }
                catch (Exception ex)
                {
                    if (string.IsNullOrEmpty(videoUrl))
                    {
                        Log.Error($"There is a problem to take an item from queue {ex}");
                        continue;
                    }
                    if (ex is YoutubeParseException)
                    {
                        Log.Error(
                                $"Video was removed or blocked | url={videoUrl}\n{ex}");
                        _videosProgress[videoUrl].Stage = VideoProgressStage.VideoUrlProblem;
                        _videosProgress[videoUrl].ErrorArgs = ex.ToString();
                        continue;
                    }

                    if (ex is IOException || ex is WebException)
                    {
                        Log.Error(
                            $"Problem with downloading video | url={videoUrl} | Retries={_videosProgress[videoUrl].Retries}\n{ex}");
                    }
                    else
                    {
                        Log.Error($"Not expected case{ex}");
                    }
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
