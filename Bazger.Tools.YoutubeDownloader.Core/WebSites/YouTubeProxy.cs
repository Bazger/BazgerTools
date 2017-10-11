using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using NLog;
using YoutubeExtractor;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public class YouTubeProxy : IWebSiteDownloaderProxy
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly VideoType _type;
        private readonly int _resolution;
        private readonly int _retriesCount;

        public YouTubeProxy(VideoType type = VideoType.Mp4, int resolution = 360, int retriesCount = 3)
        {
            _type = type;
            _resolution = resolution;
            _retriesCount = retriesCount;
        }

        public void Download(VideoProgressMetadata videoProgress)
        {
            List<VideoInfo> videoInfos = new List<VideoInfo>();
            videoInfos = DownloadUrlResolver.GetDownloadUrls(videoProgress.Url, false).ToList();

            /*
             * Select the first .mp4 video with 360p resolution
             */
            var video = videoInfos
                .First(info => info.VideoType == _type && info.Resolution >= _resolution);

            if (video == null)
            {
                video = videoInfos
                    .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
            }

            /*
             * If the video has a decrypted signature, decipher it
             */
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            videoProgress.Title = video.Title;

            /*
             * Create the video downloader.
             * The first argument is the video to download.
             * The second argument is the path to save the video file.
             */
            var videoPath = Path.Combine(videoProgress.OutputDirectory,
                RemoveIllegalPathCharacters(video.Title) + video.VideoExtension);
            videoProgress.VideoFilePath = videoPath;
            var videoDownloader = new VideoDownloader(video, videoPath);


            // Register the ProgressChanged event and print the current progress
            videoDownloader.DownloadProgressChanged += (sender, args) =>
               videoProgress.Progress = Math.Round(args.ProgressPercentage, 2);

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
                Log.Warn($"Can't download video, will retry | Url={videoProgress.Url} | Retries count={_retriesCount} \n" + ex);
            }
            RetryToDownload(videoDownloader, videoProgress);
        }

        private void RetryToDownload(Downloader videoDownloader, VideoProgressMetadata videoProgress)
        {
            while (videoProgress.Retries <= _retriesCount)
            {
                try
                {
                    videoProgress.Retries++;
                    videoDownloader.Execute();
                    break;
                }
                catch (Exception ex)
                {
                    if (videoProgress.Retries <= _retriesCount)
                    {
                        Log.Warn(
                            $"Can't download video | url={videoProgress.Url} | Retries={videoProgress.Retries} \n" +
                            ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex($"[{Regex.Escape(regexSearch)}]");
            return r.Replace(path, "");
        }
    }
}
