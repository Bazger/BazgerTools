using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Converters
{
    public class FFmpegConverterProcessProxy : ExternalProcessProxy
    {
        private readonly VideoProgressMetadata _videoMetadata;
        private readonly int _wairingTimeout;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public FFmpegConverterProcessProxy(VideoProgressMetadata videoMetadata, int wairingTimeout)
        {
            _videoMetadata = videoMetadata;
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
                throw new ExternalException($"External process error occured | Bad exit_code={ExternalProcess.ExitCode}");
            }
            base.Stop();
            throw new ExternalException($"Timeout expired. FFmpeg has problem with converting this video | timeout={_wairingTimeout}");
        }
    }
}
