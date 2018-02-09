using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bazger.Tools.ObnulAtor.Utils;

namespace Bazger.Tools.ObnulAtor
{
    public class KasperskyResetTrialVersion5 : KasperskyResetTrialBase
    {
        public KasperskyResetTrialVersion5(string windowTitle, string executableName) : base(windowTitle, executableName)
        {
        }

        public override bool ResetActivation()
        {
            if (Hwnd == IntPtr.Zero)
            {
                //TODO: log here
                return false;
            }
            var hwndChild = User32.FindWindowEx(Hwnd, IntPtr.Zero, "TButton", "Сбросить активацию");
            if (hwndChild == IntPtr.Zero)
            {
                //TODO: log here
                return false;
            }

            User32.PostMessage(hwndChild, User32.BN_CLICKED, 0, IntPtr.Zero);
            Thread.Sleep(200);
            foreach (var hwnd in WindowsHelper.FindWindowsWithText(WindowTitle))
            {
                var hWndMsgBox = User32.FindWindowEx(hwnd, IntPtr.Zero, "Button", "&Да");
                if (hWndMsgBox != IntPtr.Zero)
                {
                    User32.SendMessage(hWndMsgBox, User32.WM_LBUTTONDOWN, 0, IntPtr.Zero);
                    User32.SendMessage(hWndMsgBox, User32.WM_LBUTTONUP, 0, IntPtr.Zero);
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
