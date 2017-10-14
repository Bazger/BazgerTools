using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.Converters;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class ConverterThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly BlockingCollection<VideoProgressMetadata> _waitingForConvertion;
        private readonly BlockingCollection<VideoProgressMetadata> _waitingForMoving;
        private readonly string _convertionFormat;
        private readonly List<DownloaderThread> _downloaderThreads;
        private readonly string _launcherTempDir;
        private ExternalProcessProxy _runningProcessProxy;
        private string _converterTempDir;
        private const int MillisecondsTimeout = 5000;
        private const int ExternalConverterWaitingTimeout = 30000;

        public ConverterThread(string name, BlockingCollection<VideoProgressMetadata> waitingForConvertion, BlockingCollection<VideoProgressMetadata> waitingForMoving, string convertionFormat, List<DownloaderThread> downloaderThreads, string launcherTempDir) : base(name)
        {
            _waitingForConvertion = waitingForConvertion;
            _waitingForMoving = waitingForMoving;
            _convertionFormat = convertionFormat;
            _downloaderThreads = downloaderThreads;
            _launcherTempDir = launcherTempDir;
        }

        protected override void Job()
        {
            _converterTempDir = Path.Combine(_launcherTempDir, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_converterTempDir);

            //TODO: _downloaderThreads.Count may be replaced
            //WaitOne equals 0 because we are waiting when we taking from queue, so we dont need to wait on stop event
            while ((_waitingForConvertion.Count != 0 || _downloaderThreads.Count(c => c.IsAlive) != 0 ||
                    _downloaderThreads.FirstOrDefault(c => c.IsStarted) == null) && !StopEvent.WaitOne(0))
            {
                _runningProcessProxy = null;
                VideoProgressMetadata videoMetadata = null;
                try
                {
                    _waitingForConvertion.TryTake(out videoMetadata, MillisecondsTimeout);
                    if (videoMetadata == null)
                    {
                        continue;
                    }

                    videoMetadata.Stage = VideoProgressStage.Converting;
                    videoMetadata.ConverterTempDir = _converterTempDir;
                    videoMetadata.ConvertedFilePath = Path.Combine(_converterTempDir, Guid.NewGuid() + $".{_convertionFormat}");
                    _runningProcessProxy = new FFmpegConverterProcessProxy(videoMetadata,
                        ExternalConverterWaitingTimeout);
                    Log.Info(LogHelper.Format("Converting video", videoMetadata));
                    _runningProcessProxy.Start();
                    Log.Info(LogHelper.Format("Video successfully converted", videoMetadata));

                    videoMetadata.MovingFilePath = videoMetadata.ConvertedFilePath;
                    _waitingForMoving?.TryAdd(videoMetadata);
                    videoMetadata.Stage = VideoProgressStage.Moving;
                }
                catch (Exception ex)
                {
                    if (videoMetadata == null)
                    {
                        continue;
                    }
                    Log.Error(ex,
                        LogHelper.Format($"Can't convert video | path={videoMetadata.VideoFilePath}",
                            videoMetadata));
                    //TODO: Remove video file if err occured
                    videoMetadata.Stage = VideoProgressStage.Error;
                    videoMetadata.ErrorArgs = ex.ToString();
                }
            }
            StoppedEvent.Set();
        }

        public override void Start()
        {
            StopEvent = new ManualResetEvent(false);
            StoppedEvent = new ManualResetEvent(false);

            JobThread.Start();
            IsStarted = true;
        }

        public override void Stop()
        {
            StopEvent.Set();
        }

        public override void Abort()
        {
            Log.Warn($"Abort converter service ({Name})");
            _runningProcessProxy?.Stop();
            JobThread.Abort();
        }
    }
}
