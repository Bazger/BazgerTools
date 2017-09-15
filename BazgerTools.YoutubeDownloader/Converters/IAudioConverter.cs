using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazgerTools.YouTubeDownloader.Converters
{
    public interface IAudioConverter
    {
        string Convert(string path, string format);
    }
}
