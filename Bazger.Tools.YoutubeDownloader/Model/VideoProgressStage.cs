using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazger.Tools.YouTubeDownloader.Model
{
    public enum VideoProgressStage
    {
        Downloading,
        Exist,
        WaitingToConvertion, 
        Converting,
        Completed,
        Error
    }
}
