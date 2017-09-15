using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazgerTools.YouTubeDownloader.Model
{
    public class VideoProgressMetadata
    {
        public VideoProgressStage Stage { get; set; }
        public string Title { get; set; }
        public double Progress { get; set; }
        public string ErrorArgs { get; set; }
        public int Retries { get; set; }
    }
}
