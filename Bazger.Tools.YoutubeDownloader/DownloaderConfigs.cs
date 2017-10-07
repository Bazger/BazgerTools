using System;
using System.Configuration;
using System.IO;

namespace Bazger.Tools.YouTubeDownloader.Core
{
    public class DownloaderConfigs : ConfigurationSection
    {
        public static DownloaderConfigs GetDefaultConfigs()
        {
            return new DownloaderConfigs()
            {
                SaveDir = "Downloads",
                ParallelDownloadsCount = 5,
                ConvertersCount = 3,
                ConverterEnabled = true,
                ConvertionFormat = "wav",
                JournalFileName = "Journal.json",
                WriteToJournal = false,
                ReadFromJournal = false
            };
        }


        public static DownloaderConfigs GetConfig()
        {
            return (DownloaderConfigs)ConfigurationManager.GetSection("DownloaderConfigs") ?? new DownloaderConfigs();
        }

        [ConfigurationProperty("youTubeApiKey", IsRequired = true)]
        public string YouTubeApiKey
        {
            get
            {
                return (string)this["youTubeApiKey"];
            }
            set
            {
                this["youTubeApiKey"] = value;
            }
        }

        [ConfigurationProperty("saveDir", IsRequired = true)]
        public string SaveDir
        {
            get
            {
                return (string)this["saveDir"];
            }
            set
            {
                if (value == null)
                {
                    this["saveDir"] = Path.Combine(Environment.CurrentDirectory, "Downloads");
                }
                else
                {
                    this["saveDir"] = value;
                }

            }
        }

        [ConfigurationProperty("downloadUrl", IsRequired = true)]
        public string DownloadUrl
        {
            get
            {
                return (string)this["downloadUrl"];
            }
            set
            {
                this["downloadUrl"] = value;
            }
        }

        [ConfigurationProperty("parallelDownloadsCount", IsRequired = false, DefaultValue = 10)]
        public int ParallelDownloadsCount
        {
            get
            {
                return (int)this["parallelDownloadsCount"];
            }
            set
            {
                this["parallelDownloadsCount"] = value;
            }
        }

        [ConfigurationProperty("convertersCount", IsRequired = false, DefaultValue = 4)]
        public int ConvertersCount
        {
            get
            {
                return (int)this["convertersCount"];
            }
            set
            {
                this["convertersCount"] = value;
            }
        }

        [ConfigurationProperty("converterEnabled", IsRequired = false, DefaultValue = true)]
        public bool ConverterEnabled
        {
            get
            {
                return (bool)this["converterEnabled"];
            }
            set
            {
                this["converterEnabled"] = value;
            }
        }

        [ConfigurationProperty("convertionFormat", IsRequired = false, DefaultValue = "mp3")]
        public string ConvertionFormat
        {
            get
            {
                return (string)this["convertionFormat"];
            }
            set
            {
                this["convertionFormat"] = value;
            }
        }

        [ConfigurationProperty("journalFileName", IsRequired = true)]
        public string JournalFileName
        {
            get
            {
                return (string)this["journalFileName"];
            }
            set
            {
                if (value == null)
                {
                    this["journalFileName"] = "Journal.json";
                }
                else
                {
                    this["journalFileName"] = value;
                }
            }
        }

        [ConfigurationProperty("readFromJournal", IsRequired = false, DefaultValue = false)]
        public bool ReadFromJournal
        {
            get
            {
                return (bool)this["readFromJournal"];
            }
            set
            {
                this["readFromJournal"] = value;
            }
        }

        [ConfigurationProperty("writeToJournal", IsRequired = false, DefaultValue = false)]
        public bool WriteToJournal
        {
            get
            {
                return (bool)this["writeToJournal"];
            }
            set
            {
                this["writeToJournal"] = value;
            }
        }
    }
}
