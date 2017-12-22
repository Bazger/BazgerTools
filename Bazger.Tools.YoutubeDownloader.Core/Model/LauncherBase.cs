using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public abstract class LauncherBase : ServiceThread
    {
        //TODO: Check how logger works
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected LauncherBase(string name) : base(name)
        {
        }

        protected static void StartServices(IEnumerable<ServiceThread> services)
        {
            var serviceThreads = services as IList<ServiceThread> ?? services.ToList();
            foreach (var service in serviceThreads.Where(c => !c.IsAlive && c.IsEnabled))
            {
                try
                {
                    Log.Debug("Starting service ({0})", service.Name);
                    service.Start();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error starting service ({0})", service.Name);
                }
            }
            Log.Info("{0} serveice threads was started", serviceThreads.Count(c => c.IsStarted));
        }

        protected static void StopServices(IEnumerable<ServiceThread> services)
        {
            foreach (var service in services.Reverse())
            {
                try
                {
                    Log.Debug("Stopping service ({0})", service.Name);
                    service.Stop();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error stopping service ({0})", service.Name);
                }
            }
        }

        protected static void AbortServices(IEnumerable<ServiceThread> services)
        {
            foreach (var service in services.Reverse())
            {
                service.Abort();
            }
        }
    }
}
