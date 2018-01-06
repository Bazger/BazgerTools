using System.Threading;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public abstract class ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public string Name => JobThread.Name;

        protected readonly Thread JobThread;
        protected ManualResetEvent StopEvent;
        protected ManualResetEvent StoppedEvent;

        public bool IsAlive => JobThread?.IsAlive ?? false;

        public bool IsEnabled { get; protected set; }
        public bool IsStarted { get; protected set; }

        protected ServiceThread(string name)
        {
            IsEnabled = true;
            JobThread = new Thread(Job)
            {
                Name = name,
            };
        }

        public void Start()
        {
            if (JobThread.IsAlive)
            {
                throw new ThreadStateException($"{Name} service already started");
            }
            StopEvent = new ManualResetEvent(false);
            StoppedEvent = new ManualResetEvent(false);

            JobThread.Start();
            IsStarted = true;
        }

        public virtual void Stop()
        {
            StopEvent.Set();
            StoppedEvent.Set();
        }

        protected virtual void Job()
        {
            StoppedEvent.Set();
        }

        public virtual void Abort()
        {
            if (!JobThread.IsAlive)
            {
                return;
            }
            Log.Warn($"Abort service ({Name})");
            JobThread.Abort();
        }

        public bool Wait(int waitTimeout = -1)
        {
            return StoppedEvent.WaitOne(waitTimeout);
        }
    }
}
