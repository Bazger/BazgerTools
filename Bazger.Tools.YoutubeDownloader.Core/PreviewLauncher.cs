using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core
{
    public class PreviewLauncher : LauncherBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IEnumerable<string> _videoUrls;
        private readonly VideoTypeIds _selectedVideoTypeId;
        private readonly List<ServiceThread> _previewThreads;

        public PreviewLauncher(IEnumerable<string> videoUrls, VideoTypeIds selectedVideoTypeId, string name = "PreviewLauncher") : base(name)
        {
            _videoUrls = videoUrls;
            _selectedVideoTypeId = selectedVideoTypeId;
            _previewThreads = new List<ServiceThread>();
        }

        protected override void Job()
        {
            VideosProgress.Clear();
            _previewThreads.Clear();

            //STAGE I - Initializing
            StartupVideosProgress();
            var waitingForGettingPreview = new BlockingCollection<VideoProgressMetadata>(new ConcurrentQueue<VideoProgressMetadata>(_videoUrls.Select(url => VideosProgress[url])));
            StartPreviewThreads(waitingForGettingPreview);
            if (StopEvent.WaitOne(0))
            {
                return;
            }

            //STAGE II - Processing
            //Check if threads downloading and converting
            while (_previewThreads.Count(c => c.IsAlive) != 0)
            {
                //Check if stop of thread called
                if (StopEvent.WaitOne(1000))
                {
                    return;
                }
            }
            Log.Info($"{Name} finished its work");
            base.Job();
        }

        public override void Stop()
        {
            if (StoppedEvent.WaitOne(0))
            {
                Log.Info($"{Name} service already stopped");
                return;
            }
            if (StopEvent.WaitOne(0))
            {
                Log.Info($"{Name} service is stopping now");
                return;
            }
            StopEvent.Set();
            StopServices(_previewThreads);
            var waitEvent = new AutoResetEvent(false);
            while (!waitEvent.WaitOne(2000))
            {
                if (_previewThreads.Count(c => c.IsAlive) == 0)
                {
                    waitEvent.Set();
                    break;
                }
                AbortServices(_previewThreads);
            }

            StoppedEvent.Set();
            Log.Info($"{Name} service stopped");
        }

        private void StartupVideosProgress()
        {
            foreach (var url in _videoUrls)
            {
                var progressMetadata = new VideoProgressMetadata(url);
                VideosProgress.TryAdd(url, progressMetadata);
            }
        }


        private void StartPreviewThreads(BlockingCollection<VideoProgressMetadata> waitingForGettingPreview)
        {
            //Inialing once because method are safe for multithreaded usage
            for (var i = 0; i < 10; i++)
            {
                _previewThreads.Add(new PreviewThread($"Preview {i}", waitingForGettingPreview, VideoType.AvailabledVideoTypesDictionary[_selectedVideoTypeId]));
            }

            Log.Info("Starting preview threads");
            StartServices(_previewThreads);
            Log.Info("Preview threads was started");
        }


        public override void Abort()
        {
            if (!JobThread.IsAlive)
            {
                return;
            }
            Log.Warn($"Abort launcher service ({Name})");
            JobThread.Abort();
        }
    }
}
