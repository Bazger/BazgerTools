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
        //Don't forget to set videoMetadata variables:
        //- videoMetadata.Title
        //- videoMetadata.PossibleVideoTypes
        //- videoMetadata.SelecetedVideoType
        void Preview(VideoProgressMetadata videoMetadata);
    }
}
