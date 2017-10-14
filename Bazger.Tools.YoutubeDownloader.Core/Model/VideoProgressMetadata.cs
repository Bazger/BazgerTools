﻿namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class VideoProgressMetadata
    {
        public VideoProgressMetadata(string url)
        {
            Url = url;
        }

        public VideoProgressStage Stage { get; set; }
        public string Title { get; set; }
        public double Progress { get; set; }
        public string ErrorArgs { get; set; }
        public int Retries { get; set; }
        public string VideoFilePath { get; set; }
        public string ConvertedFilePath { get; set; }
        public string MovingFilePath { get; set; }
        public string SaveDir { get; set; }
        public string Url { get; private set; }
        public string DownloaderTempDir { get; set; }
        public string ConverterTempDir { get; set; }
    }
}
