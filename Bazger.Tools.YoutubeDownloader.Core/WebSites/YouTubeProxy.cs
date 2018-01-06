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
    public class YouTubeProxy : WebSiteDownloaderProxy, IPreviewVideoProxy
    {
        private const int DefaultVideoFormatCode = 18;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly int _videoFormatCode;


        public YouTubeProxy(int videoFormatCode = DefaultVideoFormatCode, int retriesCount = 3) : base(retriesCount)
        {
            _videoFormatCode = videoFormatCode;
        }

        public void Preview(VideoProgressMetadata videoMetadata)
        {
            videoMetadata.VideoInfos = DownloadUrlResolver.GetDownloadUrls(videoMetadata.Url, false).ToList();
        }

        public void DownloadFromPriview(VideoProgressMetadata videoMetadata)
        {
            var video = videoMetadata.SelectedVideoInfo;

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
            RetryToDownload(() => { videoDownloader.Execute(); }, videoMetadata);
        }

        public override void Download(VideoProgressMetadata videoMetadata)
        {
            var videoInfos = DownloadUrlResolver.GetDownloadUrls(videoMetadata.Url, false).ToList();

            /*
             * Select the first .mp4 video with 360p resolution
             */
            var video = videoInfos
                .First(info => info.FormatCode == _videoFormatCode) ?? videoInfos
                    .First(info => info.FormatCode == DefaultVideoFormatCode);

            if (video == null)
            {
                throw new Exception("No VideoInfo to pick. Tried to chose default one, but failed");
            }
            videoMetadata.SelectedVideoInfo = video;

            DownloadFromPriview(videoMetadata);
        }
    }
}
