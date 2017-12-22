using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bazger.Tools.YouTubeDownloader.Core.Model;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public interface IPreviewVideoProxy
    {
        void Preview(VideoProgressMetadata videoMetadata);
        void DownloadFromPriview(VideoProgressMetadata videoMetadata);
    }
}
