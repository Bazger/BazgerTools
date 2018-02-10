using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bazger.Tools.WinApi;

namespace Bazger.Tools.ObnulAtor
{
    public abstract class KasperskyResetTrialBase
    {
        private const int RetriesCount = 3;

        protected readonly string WindowTitle;
        private readonly string _executableName;
        protected IntPtr Hwnd;

        protected KasperskyResetTrialBase(string windowTitle, string executableName)
        {
            WindowTitle = windowTitle;
            _executableName = executableName;
        }

        public bool Open()
        {
            var hwnd = User32.FindWindow(null, WindowTitle);
            if (hwnd != IntPtr.Zero)
            {
                Hwnd = hwnd;
                return true;
            }
            try
            {
                Process.Start(_executableName);
                var retriesCount = 0;
                var stopEvent = new ManualResetEvent(false);
                while (!stopEvent.WaitOne(500) && retriesCount < RetriesCount)
                {
                    hwnd = User32.FindWindow(null, WindowTitle);
                    if (hwnd != IntPtr.Zero)
                    {
                        Hwnd = hwnd;
                        return true;
                    }
                    retriesCount++;
                }
            }
            catch (Exception)
            {
                //TODO: log here
                // ignored
            }
            return false;
        }

        public bool Close()
        {
            //Get a handle for the Calculator Application main window
            var hwnd = User32.FindWindow(null, WindowTitle);

            //send WM_CLOSE system message
            if (hwnd == IntPtr.Zero) { return false; }
            User32.SendMessage(hwnd, WM.CLOSE, 0, IntPtr.Zero);
            return true;
        }

        public abstract bool ResetActivation();
        public abstract bool ActivateFromLicence();
        public abstract bool SaveLicence();
        public abstract string GetVersion();
    }
}
