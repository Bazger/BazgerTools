using System.Diagnostics;
using System.IO;

namespace Bazger.Tools.YouTubeDownloader.Core.Converters
{
    public class FFmpegConverterProxy : IAudioConverterProxy
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
