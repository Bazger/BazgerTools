using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;
using YoutubeExtractor;
using VideoType = Bazger.Tools.YouTubeDownloader.Core.Model.VideoType;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public class YouTubeProxy : WebSiteDownloaderProxy, IPreviewVideoProxy
    {
        private readonly VideoType _defualtVideoType;
        //TODO: Change for normal thing

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // Key is BazgerToolsModel.VideoInfo id; Value - YoutubeExtractor.VideoInfos ids
        private static readonly Dictionary<int, List<int>> VideosMappings = new Dictionary<int, List<int>>
        {
            {1080, new List<int>{137} },
            {720, new List<int>{22,136} },
            {480, new List<int>{135} },
            {360, new List<int>{18,134} },
            {240, new List<int>{133} },
            {144, new List<int>{160} },
            {0, new List<int>{140} }
        };

        public YouTubeProxy(VideoType defualtVideoType, int retriesCount = 3) : base(retriesCount)
        {
            _defualtVideoType = defualtVideoType;
        }

        public void Preview(VideoProgressMetadata videoMetadata)
        {
            var videoInfos = DownloadUrlResolver.GetDownloadUrls(videoMetadata.Url, false).ToList();
            var formatCodes = videoInfos.Select(v => v.FormatCode);

            videoMetadata.PossibleVideoTypes = VideosMappings.Where(pair => pair.Value.Any(formatCode => formatCodes.Contains(formatCode))).
                Select(pair => VideoType.AvailabledVideoTypes.First(v => v.Id == pair.Key));

            videoMetadata.Title = videoInfos.First()?.Title;
        }


        public override void Download(VideoProgressMetadata videoMetadata)
        {
            var videosInfo = DownloadUrlResolver.GetDownloadUrls(videoMetadata.Url, false).ToList();

            var selectedVideoType = videoMetadata.SelectedVideoType ?? _defualtVideoType;

            /*
             * Try find select video
             */
            VideoInfo videoInfo = null;
            foreach (var formatCode in VideosMappings[selectedVideoType.Id])
            {
                videoInfo = videosInfo.First(v => v.FormatCode == formatCode);
                if (videoInfo != null)
                {
                    break;
                }
            }

            //TODO: May be priority download for video if not exists
            if (videoInfo == null)
            {
                throw new Exception("No VideoInfo to pick. Tried to chose default one, but failed");
            }

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (videoInfo.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
            }

            if (videoMetadata.Title != null)
            {
                videoMetadata.Title = videoInfo.Title;
            }

            /*
             * Create the video downloader.
             * The first argument is the video to download.
             * The second argument is the path to save the video file.
             */
            videoMetadata.VideoFilePath = Path.Combine(videoMetadata.DownloaderTempDir,
                Guid.NewGuid() + videoInfo.VideoExtension);
            var videoDownloader = new VideoDownloader(videoInfo, videoMetadata.VideoFilePath);


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
    }
}
