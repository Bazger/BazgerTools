﻿namespace Bazger.Tools.YouTubeDownloader.Core.Model
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
