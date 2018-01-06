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
        private readonly string _launcherTempDir;
        private ExternalProcessProxy _runningProcessProxy;
        private string _converterTempDir;
        private const int MillisecondsTimeout = 5000;
        private const int ExternalConverterWaitingTimeout = 30000;

        public ConverterThread(string name, BlockingCollection<VideoProgressMetadata> waitingForConvertion, BlockingCollection<VideoProgressMetadata> waitingForMoving, string convertionFormat, string launcherTempDir) : base(name)
        {
            _waitingForConvertion = waitingForConvertion;
            _waitingForMoving = waitingForMoving;
            _convertionFormat = convertionFormat;
            _launcherTempDir = launcherTempDir;
        }

        protected override void Job()
        {
            _converterTempDir = Path.Combine(_launcherTempDir, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_converterTempDir);

            //WaitOne equals 0 because we are waiting when we taking from queue, so we dont need to wait on stop event
            while (!StopEvent.WaitOne(0))
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
                    videoMetadata.ConverterTempDir = _converterTempDir;
                    videoMetadata.ConvertedFilePath = Path.Combine(_converterTempDir, Guid.NewGuid() + $".{_convertionFormat}");
                    videoMetadata.Stage = VideoProgressStage.Converting;
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
                    Log.Error(ex, LogHelper.Format($"Can't convert video | path={videoMetadata.VideoFilePath}", videoMetadata));
                    try
                    {
                        if (File.Exists(videoMetadata.VideoFilePath))
                        {
                            File.Delete(videoMetadata.VideoFilePath);
                        }
                    }
                    catch (Exception innerEx)
                    {
                        Log.Warn(innerEx, LogHelper.Format($"Can't delete video file | path={videoMetadata.VideoFilePath}", videoMetadata));
                    }
                    videoMetadata.Stage = VideoProgressStage.Error;
                    videoMetadata.ErrorArgs = ex.ToString();
                }
            }
            base.Job();
        }
    }
}
