using Bazger.Tools.YouTubeDownloader.Core.Model;

namespace Bazger.Tools.YouTubeDownloader.Core.Converters
{
    public interface IAudioConverterProxy
    {
        void Convert(VideoProgressMetadata videoMetadata);
    }
}
