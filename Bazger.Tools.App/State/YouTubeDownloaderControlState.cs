using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Newtonsoft.Json;

namespace Bazger.Tools.App.State
{
    [Serializable]
    public class YouTubeDownloaderControlState : IControlState
    {
        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("DownloadersThreadSpin")]
        public int DownloadersThreadSpin { get; set; }

        [JsonProperty("ConvertersThreadSpin")]
        public int ConvertersThreadSpin { get; set; }

        [JsonProperty("PreviewThreadSpin")]
        public int PreviewThreadSpin { get; set; }

        [JsonProperty("IsConversionChecked")]
        public bool IsConversionChecked { get; set; }

        [JsonProperty("ConvertionFormat")]
        public int ConvertionFormat { get; set; }

        [JsonProperty("IsWriteToJournalCheked")]
        public bool IsWriteToJournalCheked { get; set; }

        [JsonProperty("IsReadFromJournalChecked")]
        public bool IsReadFromJournalChecked { get; set; }

        [JsonProperty("IsOverwriteChecked")]
        public bool IsOverwriteChecked { get; set; }

        [JsonProperty("VideoType")]
        public int VideoTypeId { get; set; }

        [JsonProperty("JournalFilePath")]
        public string JournalFilePath { get; set; }

        [JsonProperty("DownloadsFolderPath")]
        public string DownloadsFolderPath { get; set; }
    }
}
