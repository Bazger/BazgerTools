using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core.Converters;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class ConverterThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAudioConverterProxy _audioConverterProxy;
        private readonly BlockingCollection<VideoProgressMetadata> _waitingForConvertion;
        private readonly ConcurrentDictionary<string, VideoProgressMetadata> _videosProgress;
        private readonly string _convertionFormat;
        private readonly List<DownloaderThread> _downloaderThreads;
        private ManualResetEvent _stoppedEvent;
        private VideoProgressMetadata _jobVideoProgress;
        private const int MillisecondsTimeout = 5000;

        public ConverterThread(string name, IAudioConverterProxy audioConverterProxy, BlockingCollection<VideoProgressMetadata> waitingForConvertion, ConcurrentDictionary<string, VideoProgressMetadata> videosProgress, string convertionFormat, List<DownloaderThread> downloaderThreads) : base(name)
        {
            _audioConverterProxy = audioConverterProxy;
            _waitingForConvertion = waitingForConvertion;
            _videosProgress = videosProgress;
            _convertionFormat = convertionFormat;
            _downloaderThreads = downloaderThreads;
        }

        protected override void Job()
        {
            //WaitOne equals 0 because we are waiting when we taking from queue, so we dont need to wait on stop event
            while ((_waitingForConvertion.Count != 0 || _downloaderThreads.Count(c => c.IsAlive) != 0 || _downloaderThreads.FirstOrDefault(c => c.IsStarted) == null) && !StopEvent.WaitOne(0))
            {
                _jobVideoProgress = null;
                try
                {
                    _waitingForConvertion.TryTake(out _jobVideoProgress, MillisecondsTimeout);

                    if (_jobVideoProgress == null)
                    {
                        continue;
                    }
                    _jobVideoProgress.Stage = VideoProgressStage.Converting;
                    _audioConverterProxy.Convert(_jobVideoProgress.VideoFilePath, _convertionFormat);
                    File.Delete(_jobVideoProgress.VideoFilePath);
                    _jobVideoProgress.Stage = VideoProgressStage.Completed;
                }
                catch (Exception ex)
                {
                    if (_jobVideoProgress == null)
                    {
                        continue;
                    }
                    Log.Error($"Can't convert video | url={_jobVideoProgress.Url} | path={_jobVideoProgress.VideoFilePath} \n" + ex);
                    _jobVideoProgress.Stage = VideoProgressStage.Error;
                    _jobVideoProgress.ErrorArgs = ex.ToString();
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
                if (!_stoppedEvent.WaitOne(5000))
                {
                    Log.Warn("Abort converter thread");
                    JobThread.Abort();
                }
            }
        }
    }
}
