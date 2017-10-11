using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bazger.Tools.YouTubeDownloader.Core.Converters
{
    public interface IAudioExternalConverterProxy : IAudioConverterProxy
    {
        void Terminate();
    }
}
