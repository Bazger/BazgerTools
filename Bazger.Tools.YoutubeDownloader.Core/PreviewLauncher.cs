using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core
{
    public class PreviewLauncher : LauncherBase
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ConcurrentDictionary<string, VideoProgressMetadata> VideosProgress { get; }

        private readonly IEnumerable<string> _videoUrls;
        private readonly List<ServiceThread> _previewThreads;

        public PreviewLauncher(IEnumerable<string> videoUrls, string name = "PreviewLauncher") : base(name)
        {
            _videoUrls = videoUrls;
            VideosProgress = new ConcurrentDictionary<string, VideoProgressMetadata>();
            _previewThreads = new List<ServiceThread>();
        }

        protected override void Job()
        {
            VideosProgress.Clear();
            _previewThreads.Clear();

            var waitingForGettingPreview = new BlockingCollection<VideoProgressMetadata>();

            if (StopEvent.WaitOne(0))
            {
                return;
            }

            StartPreviewThreads(waitingForGettingPreview);
        }


        private void StartPreviewThreads(BlockingCollection<VideoProgressMetadata> waitingForGettingPreview)
        {
            //Inialing once because method are safe for multithreaded usage
            for (var i = 0; i < 10; i++)
            {
                _previewThreads.Add(new PreviewThread($"Preview {i}", waitingForGettingPreview));
            }

            Log.Info("Starting downloader threads");
            StartServices(_previewThreads);
            Log.Info("Downloader threads was started");
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
