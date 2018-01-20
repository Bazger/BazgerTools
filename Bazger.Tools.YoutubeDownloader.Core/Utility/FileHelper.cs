using System;
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
    }
}
