using System.Collections.Concurrent;
using System.Collections.Generic;
using Bazger.Tools.YouTubeDownloader.Model;

namespace Bazger.Tools.YouTubeDownloader.WebSites
{
    public interface IWebSiteDownloader
    {
        string Download(string videoUrl, string savePath, VideoProgressMetadata metadata);
    }
}
