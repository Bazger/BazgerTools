using System.Threading;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public abstract class ServiceThread
    {
        public string Name => JobThread.Name;

        protected readonly Thread JobThread;
        protected ManualResetEvent StopEvent;

        public bool IsAlive => JobThread.IsAlive;
        public bool IsEnabled { get; private set; }
        public bool IsStarted { get; protected set; }


        protected ServiceThread(string name)
        {
            IsEnabled = true;
            JobThread = new Thread(Job)
            {
                Name = name,
            };
        }

        // Thread methods / properties
        public abstract void Start();
        public abstract void Stop();
        // Override in base class
        protected abstract void Job();
        public abstract void Abort();
    }
}
