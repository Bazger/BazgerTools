using System;
using System.Collections.Concurrent;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using Bazger.Tools.YouTubeDownloader.Core.WebSites;
using NLog;
using YoutubeExtractor;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class PreviewThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly BlockingCollection<VideoProgressMetadata> _waitingForGettingPreview;
        private readonly IPreviewVideoProxy _youTubeProxy;
        private const int MillisecondsTimeout = 5000;

        public PreviewThread(string name, BlockingCollection<VideoProgressMetadata> waitingForGettingPreview, VideoType selectedVideoType) : base(name)
        {
            _waitingForGettingPreview = waitingForGettingPreview;
            _youTubeProxy = new YouTubeExtractorProxy(selectedVideoType);
        }

        protected override void Job()
        {
            while (_waitingForGettingPreview.Count != 0 && !StopEvent.WaitOne(0))
            {
                VideoProgressMetadata videoMetadata = null;
                try
                {
                    _waitingForGettingPreview.TryTake(out videoMetadata, MillisecondsTimeout);
                    if (videoMetadata == null)
                    {
                        continue;
                    }
                    videoMetadata.Stage = VideoProgressStage.GettingPreview;
                    Log.Info(LogHelper.Format("Getting preview for video", videoMetadata));
                    _youTubeProxy.Preview(videoMetadata);
                    Log.Info(LogHelper.Format("Video preview was found", videoMetadata));
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

                    Log.Error(ex, LogHelper.Format("Can't get preview. May be has problems with connection", videoMetadata));
                    videoMetadata.Stage = VideoProgressStage.Error;
                    videoMetadata.ErrorArgs = ex.ToString();
                }
            }
            base.Job();
        }
    }
}
