using System;
using System.Configuration;
using System.IO;
using Bazger.Tools.YouTubeDownloader.Core.Model;

namespace Bazger.Tools.YouTubeDownloader.Core
{
    public class DownloaderConfigs : ConfigurationSection
    {
        public static DownloaderConfigs GetDefaultConfigs()
        {
            return new DownloaderConfigs()
            {
                YouTubeApiKey = "AIzaSyAQXaMeoVZGg5DNr5M_tgAkh28QMLb1Q6U",
                SaveDir = "Downloads",
                YouTubeVideoTypeId = VideoType.DefaultVideoType.Id,
                ParallelDownloadsCount = 10,
                ConvertersCount = 4,
                ConverterEnabled = true,
                ConvertionFormat = "wav",
                JournalFilePath = "Journal.json",
                WriteToJournal = false,
                ReadFromJournal = false,
                OverwriteEnabled = true
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

        [ConfigurationProperty("youTubeVideoTypeId", IsRequired = false, DefaultValue = VideoTypeIds.Video360P)]
        public VideoTypeIds YouTubeVideoTypeId
        {
            get
            {
                return (VideoTypeIds)this["youTubeVideoTypeId"];
            }
            set
            {
                this["youTubeVideoTypeId"] = value;
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

        [ConfigurationProperty("convertionFormat", IsRequired = false, DefaultValue = "wav")]
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

        [ConfigurationProperty("journalFilePath", IsRequired = true)]
        public string JournalFilePath
        {
            get
            {
                return (string)this["journalFilePath"];
            }
            set
            {
                if (value == null)
                {
                    this["journalFilePath"] = "Journal.json";
                }
                else
                {
                    this["journalFilePath"] = value;
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


        [ConfigurationProperty("overwriteEnabled", IsRequired = false, DefaultValue = true)]
        public bool OverwriteEnabled
        {
            get
            {
                return (bool)this["overwriteEnabled"];
            }
            set
            {
                this["overwriteEnabled"] = value;
            }
        }
    }
}
