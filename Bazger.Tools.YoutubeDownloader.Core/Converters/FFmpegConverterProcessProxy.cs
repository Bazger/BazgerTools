using System.Diagnostics;
using System.Runtime.InteropServices;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Converters
{
    public class FFmpegConverterProcessProxy : ExternalProcessProxy
    {
        private readonly int _wairingTimeout;

        public FFmpegConverterProcessProxy(VideoProgressMetadata videoMetadata, int wairingTimeout)
        {
            _wairingTimeout = wairingTimeout;
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "ffmpeg.exe",
                Arguments = $"-i \"{videoMetadata.VideoFilePath}\" \"{videoMetadata.ConvertedFilePath}\""
            };
            ExternalProcess.StartInfo = startInfo;
        }

        public override void Start()
        {
            base.Start();
            ExternalProcess.Start();
            if (ExternalProcess.WaitForExit(_wairingTimeout))
            {
                if (ExternalProcess.ExitCode == 0) { return; }
                throw new ExternalException($"{ExternalProcess.StartInfo.FileName} has problem with converting a video| Bad exitCode={ExternalProcess.ExitCode}");
            }
            base.Stop();
            throw new ExternalException($"Waiting timeout for {ExternalProcess.StartInfo.FileName} expired. | timeout={_wairingTimeout}");
        }
    }
}
