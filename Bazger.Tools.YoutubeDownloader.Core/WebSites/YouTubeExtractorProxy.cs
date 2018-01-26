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
    public class YouTubeExtractorProxy : WebSiteDownloaderProxy, IPreviewVideoProxy
    {
        private readonly VideoType _selectedVideoType;
        private readonly VideoType _defualtVideoType = VideoType.DefaultVideoType;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // Key is BazgerToolsModel.VideoInfo id; Value - YoutubeExtractor.VideoInfos ids
        private static readonly Dictionary<VideoTypeIds, List<int>> VideosMappings = new Dictionary<VideoTypeIds, List<int>>
        {
            //{VideoTypeIds.Video1080P, new List<int>{46,137} },
            {VideoTypeIds.Video720P, new List<int>{22} },
            //{VideoTypeIds.Video480P, new List<int>{135} },
            {VideoTypeIds.Video360P, new List<int>{18,43/*webm*/} }, //can be more than one value, because youtube have same(in resoulution) videos with different codes 
            {VideoTypeIds.Video240P, new List<int>{36 /*mobile*/} }, // bad sound
            {VideoTypeIds.Video144P, new List<int>{17 /*mobile*/} }, // bad sound
            {VideoTypeIds.CleanSound, new List<int>{140, 171 /*webm*/} }
        };

        public YouTubeExtractorProxy(VideoType selectedVideoType, int retriesCount = 3) : base(retriesCount)
        {
            _selectedVideoType = selectedVideoType;
        }

        public void Preview(VideoProgressMetadata videoMetadata)
        {
            var videoInfos = DownloadUrlResolver.GetDownloadUrls(videoMetadata.Url, false).ToList();
            var formatCodes = videoInfos.Select(v => v.FormatCode);

            videoMetadata.PossibleVideoTypes = (
                from pair in VideosMappings
                where pair.Value.Any(formatCode => formatCodes.Contains(formatCode))
                select VideoType.AvailabledVideoTypesDictionary[pair.Key]).ToList();

            videoMetadata.SelectedVideoType = VideoType.GetTheClosestByPrioiry(videoMetadata.PossibleVideoTypes, _selectedVideoType);

            videoMetadata.Title = videoInfos.First()?.Title;
        }

        //TODO: Functions must rerun values insted setting them
        public override void Download(VideoProgressMetadata videoMetadata)
        {
            var videosInfos = DownloadUrlResolver.GetDownloadUrls(videoMetadata.Url, false).ToList();
            var selectedVideoType = videoMetadata.SelectedVideoType ?? _selectedVideoType ?? _defualtVideoType;

            /*
             * Try find select video
             */
            VideoInfo videoInfo = null;
            var possibleVideoTypes = new List<VideoType>();
            var possibleVideosInfosDictionary = new Dictionary<VideoTypeIds, VideoInfo>();

            if (videoMetadata.SelectedVideoType == null)
            {
                foreach (var videosMapping in VideosMappings)
                {
                    VideoInfo possibleVideoInfo = null;
                    foreach (var formatCode in videosMapping.Value)
                    {
                        possibleVideoInfo = videosInfos.FirstOrDefault(v => v.FormatCode == formatCode);
                        if (possibleVideoInfo != null)
                        {
                            break;
                        }
                    }
                    if (possibleVideoInfo == null)
                    {
                        continue;
                    }
                    possibleVideoTypes.Add(VideoType.AvailabledVideoTypesDictionary[videosMapping.Key]);
                    possibleVideosInfosDictionary.Add(videosMapping.Key, possibleVideoInfo);
                }
                videoMetadata.SelectedVideoType = VideoType.GetTheClosestByPrioiry(possibleVideoTypes, _selectedVideoType);
                videoInfo = possibleVideosInfosDictionary[videoMetadata.SelectedVideoType.Id];
            }
            else
            {
                foreach (var formatCode in VideosMappings[selectedVideoType.Id])
                {
                    videoInfo = videosInfos.FirstOrDefault(v => v.FormatCode == formatCode);
                    if (videoInfo != null)
                    {
                        break;
                    }
                }
            }

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

            if (videoMetadata.Title == null)
            {
                videoMetadata.Title = videoInfo.Title;
            }

            /*
             * Create the video downloader.
             * The first argument is the video to download.
             * The second argument is the path to save the video file.
             */
            videoMetadata.DownloadedVideoFilePath = Path.Combine(videoMetadata.DownloaderTempDir,
                Guid.NewGuid() + videoInfo.VideoExtension);
            var videoDownloader = new VideoDownloader(videoInfo, videoMetadata.DownloadedVideoFilePath);


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

    // TESTING DIFFERENT FORMAT CODES
    //22 - WORK - 720
    //43 - WORK - WEBM - 360
    //18 - WORK - MP4 - 360
    //36 - WORK - 3GP - 240
    //17 - WORK - sound not so good - 3GP - 144
    //264 - NOT WORKING - no sound
    //271 - NOT WORKING - no sound
    //137 - NOT WORKING
    //248 - NOT WORKING
    //136 - NOT WORKING
    //247 - NOT WORKING - no sound
    //135 - NOT WORKING - no sound
    //244 - NOT WORKING - no sound
    //134 - NOT WORKING - no sound
    //243 - NOT WORKING - no sound
    //133 - NOT WORKING - no sound
    //242 - NOT WORKING - no sound
    //160 - NOT WORKING - no sound
    //278 - NOT WORKING - no sound
    //140 - SOUND ONLY
    //171 - SOUND ONLY
    //249 - 403 ERROR CODE
    //250 - 403 ERROR CODE
    //251 - 403 ERROR CODE
}
