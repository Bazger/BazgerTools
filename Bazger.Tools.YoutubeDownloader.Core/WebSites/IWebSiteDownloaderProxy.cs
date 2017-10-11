using Bazger.Tools.YouTubeDownloader.Core.Model;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public interface IWebSiteDownloaderProxy
    {
        void Download(VideoProgressMetadata videoMetadata);
    }
}
