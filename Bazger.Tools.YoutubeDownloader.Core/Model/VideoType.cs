using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class VideoType
    {                        
        public static readonly ReadOnlyCollection<VideoType> AvailabledVideoTypes = new List<VideoType>
        {
            new VideoType(1080, VideoFormat.Mp4, 1080),
            new VideoType(720, VideoFormat.Mp4, 720),
            new VideoType(480, VideoFormat.Mp4, 480),
            new VideoType(360, VideoFormat.Mp4, 360),
            new VideoType(240, VideoFormat.Mp4, 240),
            new VideoType(144, VideoFormat.Mp4, 144),
            new VideoType(0, VideoFormat.Mp4, 0),
        }.AsReadOnly();

        public static readonly VideoType DefaultVideoType = AvailabledVideoTypes[3];

        public int Id { get; }
        public int Resolution { get;}
        public VideoFormat Format { get; }


        public VideoType(int id, VideoFormat format, int resolution)
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
    }
}
