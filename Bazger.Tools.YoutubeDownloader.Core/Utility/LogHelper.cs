using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bazger.Tools.YouTubeDownloader.Core.Model;

namespace Bazger.Tools.YouTubeDownloader.Core.Utility
{
    public static class LogHelper
    {
        public static string Format(string message, VideoProgressMetadata videoMetadata)
        {
            return $"{message} | url={videoMetadata.Url}";
        }
    }
}
