using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
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
        private VideoProgressMetadata _currentVideoMetadata;

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
                _currentVideoMetadata = null;
                try
                {
                    string videoUrl;
                    _waitingForDownload.TryTake(out videoUrl, MillisecondsTimeout);
                    if (string.IsNullOrEmpty(videoUrl))
                    {
                        continue;
                    }
                    _currentVideoMetadata = new VideoProgressMetadata(videoUrl)
                    {
                        Stage = VideoProgressStage.Downloading,
                        Progress = 0,
                        OutputDirectory = _saveDir
                    };
                    _videosProgress.TryAdd(videoUrl, _currentVideoMetadata);
                    Log.Info(LogHelper.Format($"Downloading video", _currentVideoMetadata));
                    _webSite.Download(_currentVideoMetadata);
                    if (!_isConvertionEnabled)
                    {
                        _currentVideoMetadata.Stage = VideoProgressStage.Completed;
                        continue;
                    }
                    _waitingForConvertion?.TryAdd(_videosProgress[videoUrl]);
                    _currentVideoMetadata.Stage = VideoProgressStage.WaitingToConvertion;
                }
                catch (Exception ex)
                {
                    if (_currentVideoMetadata == null)
                    {
                        Log.Error(ex, "There is a problem to take an item from queue");
                        continue;
                    }
                    if (ex is YoutubeParseException)
                    {
                        Log.Error(ex, LogHelper.Format("Video was removed or blocked", _currentVideoMetadata));
                        _currentVideoMetadata.Stage = VideoProgressStage.VideoUrlProblem;
                        _currentVideoMetadata.ErrorArgs = ex.ToString();
                        continue;
                    }

                    if (ex is IOException || ex is WebException)
                    {
                        Log.Error(ex,
                            LogHelper.Format($"Problem with downloading video | retries={_currentVideoMetadata.Retries}", _currentVideoMetadata));
                    }
                    else
                    {
                        Log.Error(ex, "Not expected case");
                    }
                    _currentVideoMetadata.Stage = VideoProgressStage.Error;
                    _currentVideoMetadata.ErrorArgs = ex.ToString();
                    //Removing downloaded file
                    if (!string.IsNullOrEmpty(_currentVideoMetadata?.VideoFilePath) && File.Exists(_currentVideoMetadata.VideoFilePath))
                    {
                        File.Delete(_currentVideoMetadata.VideoFilePath);
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
            var waitTime = _isConvertionEnabled ? 0 : MillisecondsTimeout;
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
