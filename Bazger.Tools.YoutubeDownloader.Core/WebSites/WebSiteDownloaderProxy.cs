using System;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;
using NLog.Fluent;
using YoutubeExtractor;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public abstract class WebSiteDownloaderProxy
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly int _retriesCount;

        protected WebSiteDownloaderProxy(int retriesCount)
        {
            _retriesCount = retriesCount;
        }

        //Don't forget to set videoMetadata variables:
        //--videoMetadata.Title
        //--videoMetadata.DownloadedVideoFilePath
        //--videoMetadata.SelecetedVideoType
        //--videoMetadata.Progress
        public abstract void Download(VideoProgressMetadata videoMetadata);

        protected void RetryToDownload(Action executeAction, VideoProgressMetadata videoMetadata)
        {
            while (videoMetadata.Retries < _retriesCount)
            {
                try
                {
                    videoMetadata.Retries++;
                    executeAction();
                    break;
                }
                catch (Exception ex)
                {
                    if (videoMetadata.Retries < _retriesCount)
                    {
                        Log.Warn(ex, LogHelper.Format($"Can't download video | retry={videoMetadata.Retries}", videoMetadata));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
