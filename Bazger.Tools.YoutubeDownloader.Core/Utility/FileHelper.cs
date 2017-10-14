using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bazger.Tools.YouTubeDownloader.Core.Utility
{
    public static class FileHelper
    {
        public static string RemoveIllegalPathCharacters(string path)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var regex = new Regex($"[{Regex.Escape(regexSearch)}]");
            return regex.Replace(path, "");
        }

        //TODO: fix a bug
        public static string GetAvailableFilePath(string directory, string fileName)
        {
            var count = 2;
            var tempFileName = fileName;
            while (Directory.GetFiles(directory).Contains(tempFileName))
            {
                tempFileName = $"{fileName} ({count++})";
            }
            return tempFileName;
        }
    }
}
