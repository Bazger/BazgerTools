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
        private readonly string _convertionFormat;
        private readonly List<DownloaderThread> _downloaderThreads;
        private ManualResetEvent _stoppedEvent;
        private VideoProgressMetadata _currentVideoMetadata;
        private ExternalProcessProxy _currentProcessProxy;
        private const int MillisecondsTimeout = 5000;
        private const int ExternalConverterWaitingTimeout = 30000;

        public ConverterThread(string name, BlockingCollection<VideoProgressMetadata> waitingForConvertion, string convertionFormat, List<DownloaderThread> downloaderThreads) : base(name)
        {
            _waitingForConvertion = waitingForConvertion;
            _convertionFormat = convertionFormat;
            _downloaderThreads = downloaderThreads;
        }

        protected override void Job()
        {
            //WaitOne equals 0 because we are waiting when we taking from queue, so we dont need to wait on stop event
            while ((_waitingForConvertion.Count != 0 || _downloaderThreads.Count(c => c.IsAlive) != 0 ||
                    _downloaderThreads.FirstOrDefault(c => c.IsStarted) == null) && !StopEvent.WaitOne(0))
            {
                _currentVideoMetadata = null;
                _currentProcessProxy = null;
                try
                {
                    _waitingForConvertion.TryTake(out _currentVideoMetadata, MillisecondsTimeout);
                    if (_currentVideoMetadata == null) { continue; }

                    _currentVideoMetadata.Stage = VideoProgressStage.Converting;
                    _currentVideoMetadata.ConvertedFilePath = Path.Combine(_currentVideoMetadata.OutputDirectory,
                        Path.GetFileNameWithoutExtension(_currentVideoMetadata.VideoFilePath) + $".{_convertionFormat}");
                    _currentProcessProxy = new FFmpegConverterProcessProxy(_currentVideoMetadata, ExternalConverterWaitingTimeout);
                    //TODO: Remove before converting?
                    Log.Info(LogHelper.Format($"Converting video", _currentVideoMetadata));
                    _currentProcessProxy.Start();
                    _currentVideoMetadata.Stage = VideoProgressStage.Completed;
                    File.Delete(_currentVideoMetadata.VideoFilePath);
                    Log.Info(LogHelper.Format($"Video successfully converted", _currentVideoMetadata));
                }
                catch (Exception ex)
                {
                    if (_currentVideoMetadata == null) { continue; }
                    Log.Error(ex,
                        LogHelper.Format($"Can't convert video | path={_currentVideoMetadata.VideoFilePath}",
                            _currentVideoMetadata));
                    _currentVideoMetadata.Stage = VideoProgressStage.Error;
                    _currentVideoMetadata.ErrorArgs = ex.ToString();

                    //Removing downloaded file
                    if (File.Exists(_currentVideoMetadata.VideoFilePath))
                    {
                        File.Delete(_currentVideoMetadata.VideoFilePath);
                    }
                }
            }
            _stoppedEvent.Set();
        }

        public override void Start()
        {
            StopEvent = new ManualResetEvent(false);
            _stoppedEvent = new ManualResetEvent(false);

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
            _currentProcessProxy?.Stop();
            JobThread.Abort();
        }
    }
}
