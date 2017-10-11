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
        private readonly BlockingCollection<VideoProgressMetadata> _waitingForConvertion;
        private readonly bool _isConvertionEnabled;
        private readonly IWebSiteDownloaderProxy _webSite;
        private readonly ConcurrentDictionary<string, VideoProgressMetadata> _videosProgress;
        private readonly string _saveDir;
        private ManualResetEvent _stoppedEvent;
        private const int MillisecondsTimeout = 5000;
        private VideoProgressMetadata _jobVideoProgress;

        public DownloaderThread(string name, IWebSiteDownloaderProxy webSite, ConcurrentDictionary<string, VideoProgressMetadata> videosProgress, string saveDir, BlockingCollection<string> waitingForDownload, BlockingCollection<VideoProgressMetadata> waitingForConvertion, bool isConvertionEnabled) : base(name)
        {
            _waitingForDownload = waitingForDownload;
            _waitingForConvertion = waitingForConvertion;
            _isConvertionEnabled = isConvertionEnabled;
            _webSite = webSite;
            _videosProgress = videosProgress;
            _saveDir = saveDir;
        }


        protected override void Job()
        {
            //WaitOne equals 0 because we are waiting when we taking from queue, so we dont need to wait on stop event
            while (_waitingForDownload.Count != 0 && !StopEvent.WaitOne(0))
            {
                _jobVideoProgress = null;
                try
                {
                    string videoUrl;
                    _waitingForDownload.TryTake(out videoUrl, MillisecondsTimeout);
                    if (string.IsNullOrEmpty(videoUrl))
                    {
                        continue;
                    }
                    _jobVideoProgress = new VideoProgressMetadata(videoUrl)
                    {
                        Stage = VideoProgressStage.Downloading,
                        Progress = 0,
                        OutputDirectory = _saveDir
                    };
                    _videosProgress.TryAdd(videoUrl, _jobVideoProgress);
                    Log.Info($"Downloading video | url={videoUrl}");
                    _webSite.Download(_jobVideoProgress);
                    if (!_isConvertionEnabled)
                    {
                        _jobVideoProgress.Stage = VideoProgressStage.Completed;
                        continue;
                    }
                    _waitingForConvertion?.TryAdd(_videosProgress[videoUrl]);
                    _jobVideoProgress.Stage = VideoProgressStage.WaitingToConvertion;
                }
                catch (Exception ex)
                {
                    if (_jobVideoProgress == null)
                    {
                        Log.Error($"There is a problem to take an item from queue {ex}");
                        continue;
                    }
                    if (ex is YoutubeParseException)
                    {
                        Log.Error(
                                $"Video was removed or blocked | url={_jobVideoProgress.Url}\n{ex}");
                        _jobVideoProgress.Stage = VideoProgressStage.VideoUrlProblem;
                        _jobVideoProgress.ErrorArgs = ex.ToString();
                        continue;
                    }

                    if (ex is IOException || ex is WebException)
                    {
                        Log.Error(
                            $"Problem with downloading video | url={_jobVideoProgress.Url} | Retries={_jobVideoProgress.Retries}\n{ex}");
                    }
                    else
                    {
                        Log.Error($"Not expected case{ex}");
                    }
                    _jobVideoProgress.Stage = VideoProgressStage.Error;
                    _jobVideoProgress.ErrorArgs = ex.ToString();
                    //Removing downloaded file
                    if(!string.IsNullOrEmpty(_jobVideoProgress?.VideoFilePath) && File.Exists(_jobVideoProgress.VideoFilePath))
                    {
                        File.Delete(_jobVideoProgress.VideoFilePath);
                    }
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
            //If convertion enabled there is no purpose wait for finishig of downloader process because video will be removed 
            var waitTime = _isConvertionEnabled ? 0 : 5000;
            while (JobThread.IsAlive)
            {
                if (!_stoppedEvent.WaitOne(waitTime))
                {
                    Log.Warn("Abort downloader thread");
                    JobThread.Abort();
                }
            }
        }
    }
}
