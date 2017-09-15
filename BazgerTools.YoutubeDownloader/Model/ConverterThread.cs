using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BazgerTools.YouTubeDownloader.Converters;
using NLog;

namespace BazgerTools.YouTubeDownloader.Model
{
    public class ConverterThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IAudioConverter _audioConverter;
        private readonly BlockingCollection<Tuple<string, string>> _waitingToConvert;
        private readonly ConcurrentDictionary<string, VideoProgressMetadata> _videosProgress;
        private readonly string _convertionFormat;
        private readonly List<DownloaderThread> _downloaderThreads;
        private ManualResetEvent _stoppedEvent;
        private readonly int _millisecondsTimeout = 5000;

        public ConverterThread(string name, IAudioConverter audioConverter, BlockingCollection<Tuple<string, string>> waitingToConvert, ConcurrentDictionary<string, VideoProgressMetadata> videosProgress, string convertionFormat, List<DownloaderThread> downloaderThreads) : base(name)
        {
            _audioConverter = audioConverter;
            _waitingToConvert = waitingToConvert;
            _videosProgress = videosProgress;
            _convertionFormat = convertionFormat;
            _downloaderThreads = downloaderThreads;
        }

        protected override void Job()
        {
            while ((_waitingToConvert.Count != 0 || _downloaderThreads.Count(c => c.IsAlive) != 0 || _downloaderThreads.FirstOrDefault(c => c.IsStarted) == null) && !StopEvent.WaitOne(0))
            {
                Tuple<string, string> videoUrlAndPath = null;
                try
                {
                    _waitingToConvert.TryTake(out videoUrlAndPath, _millisecondsTimeout);

                    if (videoUrlAndPath == null)
                    {
                        continue;
                    }
                    _videosProgress[videoUrlAndPath.Item1].Stage = VideoProgressStage.Converting;
                    _audioConverter.Convert(videoUrlAndPath.Item2, _convertionFormat);
                    File.Delete(videoUrlAndPath.Item2);
                    _videosProgress[videoUrlAndPath.Item1].Stage = VideoProgressStage.Completed;
                }
                catch (Exception ex)
                {
                    if (videoUrlAndPath == null)
                    {
                        continue;
                    }
                    Log.Error($"Can't convert video | url={videoUrlAndPath.Item1} | path={videoUrlAndPath.Item1} \n" + ex);
                    _videosProgress[videoUrlAndPath.Item1].Stage = VideoProgressStage.Error;
                    _videosProgress[videoUrlAndPath.Item1].ErrorArgs = ex.ToString();
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
