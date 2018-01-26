using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using Bazger.Tools.YouTubeDownloader.Core.WebSites;
using NLog;
using YoutubeExtractor;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class DownloaderThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly BlockingCollection<VideoProgressMetadata> _waitingForDownload;
        private readonly BlockingCollection<VideoProgressMetadata> _waitingForConvertion;
        private readonly BlockingCollection<VideoProgressMetadata> _waitingForMoving;
        private readonly bool _isConvertionEnabled;
        private readonly bool _isAfterPreview;
        private readonly string _launcherTempDir;
        private string _downloaderTempDir;
        private readonly WebSiteDownloaderProxy _youTubeProxy;
        private const int MillisecondsTimeout = 5000;

        public DownloaderThread(string name,
            BlockingCollection<VideoProgressMetadata> waitingForDownload,
            BlockingCollection<VideoProgressMetadata> waitingForConvertion,
            BlockingCollection<VideoProgressMetadata> waitingForMoving,
            string launcherTempDir, bool isConvertionEnabled, bool isAfterPreview, VideoType selectedVideoType) : base(name)
        {
            _waitingForDownload = waitingForDownload;
            _waitingForConvertion = waitingForConvertion;
            _waitingForMoving = waitingForMoving;
            _isConvertionEnabled = isConvertionEnabled;
            _isAfterPreview = isAfterPreview;
            _launcherTempDir = launcherTempDir;
            _youTubeProxy = new YouTubeExtractorProxy(selectedVideoType);
        }

        protected override void Job()
        {
            _downloaderTempDir = Path.Combine(_launcherTempDir, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_downloaderTempDir);

            //WaitOne equals 0 because we are waiting when we taking from queue, so we dont need to wait on stop event
            while (_waitingForDownload.Count != 0 && !StopEvent.WaitOne(0))
            {
                VideoProgressMetadata videoMetadata = null;
                try
                {
                    _waitingForDownload.TryTake(out videoMetadata, MillisecondsTimeout);
                    if (videoMetadata == null)
                    {
                        continue;
                    }

                    videoMetadata.Stage = VideoProgressStage.Downloading;
                    videoMetadata.DownloaderTempDir = _downloaderTempDir;

                    Log.Info(LogHelper.Format("Downloading video", videoMetadata));
                    _youTubeProxy.Download(videoMetadata);
                    Log.Info(LogHelper.Format("Video successfully dwonloaded", videoMetadata));
                    if (!_isConvertionEnabled)
                    {
                        videoMetadata.MovingFilePath = videoMetadata.DownloadedVideoFilePath;
                        _waitingForMoving?.TryAdd(videoMetadata);
                        videoMetadata.Stage = VideoProgressStage.Moving;
                        continue;
                    }
                    _waitingForConvertion?.TryAdd(videoMetadata);
                    videoMetadata.Stage = VideoProgressStage.WaitingToConvertion;
                }
                catch (Exception ex)
                {
                    if (videoMetadata == null)
                    {
                        Log.Error(ex, "There is a problem to take an item from queue");
                        continue;
                    }
                    if (ex is YoutubeParseException)
                    {
                        Log.Error(ex, LogHelper.Format("Video was removed or blocked", videoMetadata));
                        videoMetadata.Stage = VideoProgressStage.VideoUrlProblem;
                        videoMetadata.ErrorArgs = ex.ToString();
                        continue;
                    }

                    if (ex is IOException)
                    {
                        Log.Error(ex,
                            LogHelper.Format($"Can't get access to file | file={videoMetadata.DownloadedVideoFilePath} | retries={videoMetadata.Retries}", videoMetadata));
                    }
                    else if (ex is WebException)
                    {
                        Log.Error(ex,
                           LogHelper.Format($"Problem with downloading video | retries={videoMetadata.Retries}", videoMetadata));
                    }
                    else
                    {
                        Log.Error(ex);
                    }
                    videoMetadata.Stage = VideoProgressStage.Error;
                    videoMetadata.ErrorArgs = ex.ToString();
                }
            }
            base.Job();
        }
    }
}
