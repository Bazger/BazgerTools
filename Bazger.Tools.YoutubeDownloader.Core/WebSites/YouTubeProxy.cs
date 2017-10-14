using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using ConcurrentCollections;
using NLog;
using YoutubeExtractor;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public class YouTubeProxy : WebSiteDownloaderProxy
    {
        private const int DefaultVideoFormatCode = 18;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly int _videoFormatCode;
        private readonly int _retriesCount;

        public YouTubeProxy( int videoFormatCode = DefaultVideoFormatCode, int retriesCount = 3) 
        {
            _videoFormatCode = videoFormatCode;
            _retriesCount = retriesCount;
        }

        public override void Download(VideoProgressMetadata videoMetadata)
        {
            List<VideoInfo> videoInfos = new List<VideoInfo>();
            videoInfos = DownloadUrlResolver.GetDownloadUrls(videoMetadata.Url, false).ToList();

            /*
             * Select the first .mp4 video with 360p resolution
             */
            var video = videoInfos
                .First(info => info.FormatCode == _videoFormatCode);

            if (video == null)
            {
                video = videoInfos
                    .First(info => info.FormatCode == DefaultVideoFormatCode);
            }

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            videoMetadata.Title = video.Title;

            /*
             * Create the video downloader.
             * The first argument is the video to download.
             * The second argument is the path to save the video file.
             */
            videoMetadata.VideoFilePath = Path.Combine(videoMetadata.DownloaderTempDir,
              Guid.NewGuid() + video.VideoExtension);
            var videoDownloader = new VideoDownloader(video, videoMetadata.VideoFilePath);


            // Register the ProgressChanged event and print the current progress
            videoDownloader.DownloadProgressChanged += (sender, args) =>
               videoMetadata.Progress = Math.Round(args.ProgressPercentage, 2);

            /*
             * Execute the video downloader.
             * For GUI applications note, that this method runs synchronously.
             */
            try
            {
                videoDownloader.Execute();
                return;
            }
            catch (Exception ex)
            {
                Log.Warn(ex, LogHelper.Format("Can't download video, will retry", videoMetadata));
            }
            RetryToDownload(videoDownloader, videoMetadata);
        }

        //TODO: Move to abstract class
        private void RetryToDownload(Downloader videoDownloader, VideoProgressMetadata videoMetadata)
        {
            while (videoMetadata.Retries < _retriesCount)
            {
                try
                {
                    videoMetadata.Retries++;
                    videoDownloader.Execute();
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
