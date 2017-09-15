using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BazgerTools.YouTubeDownloader.Converters
{
    public class FFmpegConverter : IAudioConverter
    {
        public string Convert(string path, string format)
        {
            var newMp3Path = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + $".{format}");

            var process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c ffmpeg -i \"{path}\" \"{newMp3Path}\""
            };
            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();
            return newMp3Path;
        }
    }
}
