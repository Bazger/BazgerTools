using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using Bazger.Tools.WinApi;
using NLog;

namespace Bazger.Tools.ObnulAtor
{
    public class KasperskyResetTrialVersion5 : KasperskyResetTrialBase
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public KasperskyResetTrialVersion5(string windowTitle, string executableName) : base(windowTitle, executableName)
        {
        }

        public override bool ResetActivation()
        {
            if (Hwnd == IntPtr.Zero)
            {
                _log.Warn("KRT window pointer is zero");
                return false;
            }
            var hwndChild = User32.FindWindowEx(Hwnd, IntPtr.Zero, "TButton", "Сбросить активацию");
            if (hwndChild == IntPtr.Zero)
            {
                _log.Warn("Reset Activation button pointer is zero");
                return false;
            }

            User32.PostMessage(hwndChild, BN.CLICKED, 0, IntPtr.Zero);
            Thread.Sleep(200);
            foreach (var hwnd in WindowsHelper.FindWindowsWithText(WindowTitle))
            {
                var hWndMsgBox = User32.FindWindowEx(hwnd, IntPtr.Zero, "Button", "&Да");
                if (hWndMsgBox != IntPtr.Zero)
                {
                    User32.SendMessage(hWndMsgBox, WM.LBUTTONDOWN, 0, IntPtr.Zero);
                    User32.SendMessage(hWndMsgBox, WM.LBUTTONUP, 0, IntPtr.Zero);
                    return true;
                }
            }
            return false;
        }

        public override bool ActivateFromLicence()
        {
            throw new NotImplementedException();
        }

        public override bool SaveLicence()
        {
            throw new NotImplementedException();
        }

        public override string GetVersion()
        {
            return "5.1.0.29";
        }
    }
}
