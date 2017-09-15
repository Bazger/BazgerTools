using System.Collections.Concurrent;
using System.Collections.Generic;
using BazgerTools.YouTubeDownloader.Model;

namespace BazgerTools.YouTubeDownloader.WebSites
{
    public interface IWebSiteDownloader
    {
        string Download(string videoUrl, string savePath, VideoProgressMetadata metadata);
    }
}
