using System;
using System.Collections.Concurrent;
using System.IO;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using ConcurrentCollections;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public abstract class WebSiteDownloaderProxy
    {
        public abstract void Download(VideoProgressMetadata videoMetadata);
    }
}
