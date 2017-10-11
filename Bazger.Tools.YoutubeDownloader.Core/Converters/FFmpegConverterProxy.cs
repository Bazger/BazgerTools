using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Converters
{
    public class FFmpegConverterProxy : IAudioExternalConverterProxy
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private Process _process;
        private const int ConverterWaitingTimeout = 30000;

        public void Convert(VideoProgressMetadata videoMetadata)
        {
            _process = new System.Diagnostics.Process();
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c ffmpeg -i \"{videoMetadata.VideoFilePath}\" \"{videoMetadata.ConvertedFilePath}\""
            };
            _process.StartInfo = startInfo;
            _process.Start();

            if (_process.WaitForExit(ConverterWaitingTimeout))
            {
                _process = null;
                return;
            }
            var ex = new ExternalException(LogHelper.Format($"Timeout expired. FFmpeg has problem with converting this video | timeout={ConverterWaitingTimeout}", videoMetadata));
            Log.Error(ex);
            _process.Kill();
            throw ex;
        }

        public void Terminate()
        {
            if (_process != null)
            {
                _process.Kill();
                return;
            }
            Log.Warn("No procees to terminate");
        }
    }
}
