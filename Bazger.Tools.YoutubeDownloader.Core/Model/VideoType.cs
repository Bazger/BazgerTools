using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class VideoType
    {
        public static readonly ReadOnlyCollection<VideoType> AvailabledVideoTypes = new List<VideoType>
        {
            //new VideoType(VideoTypeIds.Video1080P, VideoFormat.Mp4, 1080),
            new VideoType(VideoTypeIds.Video720P, VideoFormat.Mp4, 720),
            //new VideoType(VideoTypeIds.Video480P, VideoFormat.Mp4, 480),
            new VideoType(VideoTypeIds.Video360P, VideoFormat.Mp4, 360),
            new VideoType(VideoTypeIds.Video240P, VideoFormat.Mp4, 240),
            new VideoType(VideoTypeIds.Video144P, VideoFormat.Mp4, 144),
            new VideoType(VideoTypeIds.CleanSound, VideoFormat.Mp4, 0)
        }.AsReadOnly();

        public static readonly Dictionary<VideoTypeIds, VideoType> AvailabledVideoTypesDictionary =
            AvailabledVideoTypes.ToDictionary(v => v.Id, x => x);

        public static readonly VideoType DefaultVideoType = AvailabledVideoTypesDictionary[VideoTypeIds.Video720P];

        public VideoTypeIds Id { get; }
        public int Resolution { get; }
        public VideoFormat Format { get; }


        private VideoType(VideoTypeIds id, VideoFormat format, int resolution)
        {
            Id = id;
            Format = format;
            Resolution = resolution;
        }

        public override string ToString()
        {
            if (Resolution == 0)
            {
                return $"{Format} Clean Sound";
            }
            return $"{Format} {Resolution}p";
        }


        public static VideoType GetTheClosestByPrioiry(IEnumerable<VideoType> possibleTypes, VideoType selectedVideoType)
        {
            var start = DateTime.Now;

            if (possibleTypes.Contains(selectedVideoType))
            {
                return selectedVideoType;
            }
            var availabledVideoTypesList = AvailabledVideoTypes.ToList();
            var selectedVideoTypeIndex = AvailabledVideoTypes.IndexOf(selectedVideoType);
            //Find videos with higher options
            var optionalVideoTypes = availabledVideoTypesList.GetRange(0, selectedVideoTypeIndex);

            selectedVideoType = optionalVideoTypes.LastOrDefault(possibleTypes.Contains);
            if (selectedVideoType != null)
            {
                return selectedVideoType;
            }
            //Find videos with lower options
            optionalVideoTypes = availabledVideoTypesList
                .GetRange(selectedVideoTypeIndex + 1, availabledVideoTypesList.Count - selectedVideoTypeIndex - 1);
            selectedVideoType = optionalVideoTypes.FirstOrDefault(possibleTypes.Contains);
            Debug.WriteLine($"GetTheClosestByPrioiry {(DateTime.Now - start).Milliseconds} ms");
            return selectedVideoType; // May return null
        }

    }
}
