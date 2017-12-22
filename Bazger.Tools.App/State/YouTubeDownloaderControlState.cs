using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [JsonProperty("IsWriteToJournalCheked")]
        public bool IsWriteToJournalCheked { get; set; }

        [JsonProperty("IsReadFromJournalChecked")]
        public bool IsReadFromJournalChecked { get; set; }

        [JsonProperty("IsOverwriteChecked")]
        public bool IsOverwriteChecked { get; set; }

        [JsonProperty("VideoFormat")]
        public string VideoFormat { get; set; }

        [JsonProperty("ConvertionFormat")]
        public string ConvertionFormat { get; set; }

        [JsonProperty("JournalFilePath")]
        public string JournalFilePath { get; set; }

        [JsonProperty("DownloadsFolderPath")]
        public string DownloadsFolderPath { get; set; }
    }
}
