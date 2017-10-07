using Bazger.Tools.YouTubeDownloader.Core.Model;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public interface IWebSiteDownloaderProxy
    {
        string Download(string videoUrl, string savePath, VideoProgressMetadata metadata);
    }
}
