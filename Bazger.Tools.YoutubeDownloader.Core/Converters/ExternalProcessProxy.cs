using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Converters
{
    public abstract class ExternalProcessProxy
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        protected readonly Process ExternalProcess;
        public bool IsStarted { get; private set; }
        public bool IsAlive { get; private set; }

        protected ExternalProcessProxy()
        {
            ExternalProcess = new System.Diagnostics.Process();
            IsStarted = false;
        }

        public virtual void Start()
        {
            if (IsAlive)
            {
                throw new InvalidOperationException($"External process ({ExternalProcess.StartInfo.FileName}) already started");
            }
            IsStarted = true;
            IsAlive = true;
        }

        public virtual void Stop()
        {
            try
            {
                ProcessHelper.KillProcessAndChildrens(ExternalProcess.Id);
                ExternalProcess.WaitForExit();
                IsAlive = false;
            }
            catch (InvalidOperationException ex)
            {
                Log.Warn(ex, $"External process {ExternalProcess.StartInfo.FileName} not started yet or hase bad ProcessInfo");
            }
        }
    }
}
