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
        private readonly BlockingCollection<VideoProperties> _waitingToConvert;
        private readonly ConcurrentDictionary<string, VideoProgressMetadata> _videosProgress;
        private readonly string _convertionFormat;
        private readonly List<DownloaderThread> _downloaderThreads;
        private ManualResetEvent _stoppedEvent;
        private readonly int _millisecondsTimeout = 5000;

        public ConverterThread(string name, IAudioConverterProxy audioConverterProxy, BlockingCollection<VideoProperties> waitingToConvert, ConcurrentDictionary<string, VideoProgressMetadata> videosProgress, string convertionFormat, List<DownloaderThread> downloaderThreads) : base(name)
        {
            _audioConverterProxy = audioConverterProxy;
            _waitingToConvert = waitingToConvert;
            _videosProgress = videosProgress;
            _convertionFormat = convertionFormat;
            _downloaderThreads = downloaderThreads;
        }

        protected override void Job()
        {
            while ((_waitingToConvert.Count != 0 || _downloaderThreads.Count(c => c.IsAlive) != 0 || _downloaderThreads.FirstOrDefault(c => c.IsStarted) == null) && !StopEvent.WaitOne(0))
            {
                VideoProperties videoProp = new VideoProperties();
                try
                {
                    _waitingToConvert.TryTake(out videoProp, _millisecondsTimeout);

                    if (videoProp == null)
                    {
                        continue;
                    }
                    _videosProgress[videoProp.Url].Stage = VideoProgressStage.Converting;
                    _audioConverterProxy.Convert(videoProp.Path, _convertionFormat);
                    File.Delete(videoProp.Path);
                    _videosProgress[videoProp.Url].Stage = VideoProgressStage.Completed;
                }
                catch (Exception ex)
                {
                    if (videoProp == null)
                    {
                        continue;
                    }
                    Log.Error($"Can't convert video | url={videoProp.Url} | path={videoProp.Path} \n" + ex);
                    _videosProgress[videoProp.Url].Stage = VideoProgressStage.Error;
                    _videosProgress[videoProp.Url].ErrorArgs = ex.ToString();
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
                if (!_stoppedEvent.WaitOne(10000))
                {
                    Log.Warn("Abort worker thread");
                    JobThread.Abort();
                }
            }
        }
    }
}
