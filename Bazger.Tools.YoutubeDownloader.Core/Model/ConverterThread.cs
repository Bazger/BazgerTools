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

        private readonly IAudioExternalConverterProxy _audioConverterProxy;
        private readonly BlockingCollection<VideoProgressMetadata> _waitingForConvertion;
        private readonly string _convertionFormat;
        private readonly List<DownloaderThread> _downloaderThreads;
        private ManualResetEvent _stoppedEvent;
        private VideoProgressMetadata _currentVideoMetadata;
        private const int MillisecondsTimeout = 5000;

        public ConverterThread(string name, IAudioExternalConverterProxy audioConverterProxy, BlockingCollection<VideoProgressMetadata> waitingForConvertion, string convertionFormat, List<DownloaderThread> downloaderThreads) : base(name)
        {
            _audioConverterProxy = audioConverterProxy;
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
                try
                {
                    _waitingForConvertion.TryTake(out _currentVideoMetadata, MillisecondsTimeout);

                    if (_currentVideoMetadata == null)
                    {
                        continue;
                    }
                    _currentVideoMetadata.Stage = VideoProgressStage.Converting;
                    _currentVideoMetadata.ConvertedFilePath = Path.Combine(_currentVideoMetadata.OutputDirectory,
                        Path.GetFileNameWithoutExtension(_currentVideoMetadata.VideoFilePath) + $".{_convertionFormat}");
                    _audioConverterProxy.Convert(_currentVideoMetadata);
                    _currentVideoMetadata.Stage = VideoProgressStage.Completed;
                }
                catch (Exception ex)
                {
                    if (_currentVideoMetadata == null)
                    {
                        continue;
                    }
                    Log.Error(ex,
                        LogHelper.Format($"Can't convert video | path={_currentVideoMetadata.VideoFilePath}",
                            _currentVideoMetadata));
                    _currentVideoMetadata.Stage = VideoProgressStage.Error;
                    _currentVideoMetadata.ErrorArgs = ex.ToString();
                    //Remove bad convertion file if exception thrown
                    if (!string.IsNullOrEmpty(_currentVideoMetadata.ConvertedFilePath) && File.Exists(_currentVideoMetadata.ConvertedFilePath))
                    {
                        File.Delete(_currentVideoMetadata.ConvertedFilePath);
                    }
                }
                finally
                {
                    //Remove video file
                    if (!string.IsNullOrEmpty(_currentVideoMetadata?.VideoFilePath) && File.Exists(_currentVideoMetadata.VideoFilePath))
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
            while (JobThread.IsAlive)
            {
                if (!_stoppedEvent.WaitOne(MillisecondsTimeout))
                {
                    Log.Warn("Abort converter thread");
                    JobThread.Abort();
                    _audioConverterProxy.Terminate();
                }
            }
        }
    }
}
