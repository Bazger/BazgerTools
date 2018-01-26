using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Utility
{
    public static class FileHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static string RemoveIllegalPathCharacters(string path)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var regex = new Regex($"[{Regex.Escape(regexSearch)}]");
            return regex.Replace(path, "");
        }

        public static string GetAvailableFilePath(string futureFilePath)
        {
            var directory = Path.GetDirectoryName(futureFilePath);
            if (directory == null)
            {
                throw new NullReferenceException("Directory path can't be null");
            }
            var directoryFiles = Directory.GetFiles(directory);
            if (!directoryFiles.Contains(futureFilePath))
            {
                return futureFilePath;
            }

            var fileName = Path.GetFileNameWithoutExtension(futureFilePath);
            var fileExtension = Path.GetExtension(futureFilePath);

            string tempFilePath;
            var count = 2;
            do
            {
                tempFilePath = Path.Combine(directory, fileName + $" ({count++})" + fileExtension);
            } while (directoryFiles.Contains(tempFilePath));

            return tempFilePath;
        }

        public static IEnumerable<string> ReadFromJournal(string journalFilePath)
        {
            if (!File.Exists(journalFilePath))
            {
                Log.Warn($"Journal file doen't exist | path={journalFilePath}");
                return null;
            }
            Log.Info($"Journal file was found | path={journalFilePath}");

            try
            {
                return SerDeHelper.DeserializeJsonFile<HashSet<string>>(journalFilePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "There is a problem to read a journal file. May be journal format is illegal");
            }
            return null;
        }

        public static void WriteToJournal(string journalFilePath, List<string> videoUrls)
        {
            try
            {
                if (!File.Exists(journalFilePath))
                {
                    Log.Warn("Journal file doen't exist. Creating new one");
                    SerDeHelper.SerializeToJsonFile(new HashSet<string>(), journalFilePath);
                }
                var downloadedVideos = SerDeHelper.DeserializeJsonFile<HashSet<string>>(journalFilePath);
                videoUrls.ForEach(v => downloadedVideos.Add(v));
                SerDeHelper.SerializeToJsonFile(downloadedVideos, journalFilePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "There is a problem to write to a journal file");
            }
        }

    }
}
