using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bazger.Tools.WinApi
{
    public static class WindowsHelper
    {
        /// <summary> Get the text for the window pointed to by hWnd </summary>
        public static string GetWindowText(IntPtr hWnd)
        {
            int size = User32.GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                User32.GetWindowCaption(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        /// <summary> Find all windows that match the given filter </summary>
        /// <param name="filter"> A delegate that returns true for windows
        ///    that should be returned and false for windows that should
        ///    not be returned </param>
        private static IEnumerable<IntPtr> FindWindows(User32.EnumWindowsProc filter)
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            User32.EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                if (filter(wnd, param))
                {
                    // only add the windows that pass the filter
                    windows.Add(wnd);
                }

                // but return true here so that we iterate all windows
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        public static IEnumerable<IntPtr> GetChildWindows(IntPtr parentIntPtr)
        {
            var childWindows = new List<IntPtr>();

            User32.EnumChildWindows(parentIntPtr, delegate (IntPtr childWnd, IntPtr param)
            {
                childWindows.Add(childWnd);
                // but return true here so that we iterate all windows
                return true;
            }, IntPtr.Zero);

            return childWindows;
        }

        /// <summary> Find all windows that contain the given title text </summary>
        /// <param name="titleText"> The text that the window title must contain. </param>
        public static IEnumerable<IntPtr> FindWindowsWithText(string titleText)
        {
            return FindWindows((wnd, param) => GetWindowText(wnd).Contains(titleText));
        }

        public static IntPtr GetSystemPromotedNotificationArea()
        {
            var hWnd = User32.FindWindow("Shell_TrayWnd", null);
            hWnd = User32.FindWindowEx(hWnd, IntPtr.Zero, "TrayNotifyWnd", null);
            hWnd = User32.FindWindowEx(hWnd, IntPtr.Zero, "SysPager", null);
            hWnd = User32.FindWindowEx(hWnd, IntPtr.Zero, "ToolbarWindow32", null);
            return hWnd;
        }

        public static IntPtr GetUserPromotedNotificationArea()
        {
            var hWnd = User32.GetDesktopWindow();
            hWnd = User32.FindWindowEx(hWnd, IntPtr.Zero, "NotifyIconOverflowWindow", null);
            hWnd = User32.FindWindowEx(hWnd, IntPtr.Zero, "ToolbarWindow32", null);
            return hWnd;
        }

        public static IntPtr GetTrayWindow()
        {
            var hWnd = User32.FindWindow("Shell_TrayWnd", null);
            hWnd = User32.FindWindowEx(hWnd, IntPtr.Zero, "TrayNotifyWnd", null);
            hWnd = User32.FindWindowEx(hWnd, IntPtr.Zero, "Button", null);
            return hWnd;
        }

        /// <summary>
        /// Took from example: https://social.msdn.microsoft.com/Forums/vstudio/en-US/4a1f52ef-f74b-4055-9bfd-e69fd420338e/hide-system-tray-icons?forum=netfxbcl
        /// </summary>
        public static void DeleteAllTaskbarButtons()
        {
            IntPtr window = WindowsHelper.GetSystemPromotedNotificationArea();
            int count = (int)User32.SendMessage(window, TB.BUTTONCOUNT, IntPtr.Zero, IntPtr.Zero);

            for (int i = 0; i < count; i++)
            {
                User32.SendMessage(window, TB.DELETEBUTTON, i, 0);
            }
        }

        public static void ShowTaskbarButtons(bool showButtons)
        {
            IntPtr window = WindowsHelper.GetSystemPromotedNotificationArea();
            int count = (int)User32.SendMessage(window, TB.BUTTONCOUNT, IntPtr.Zero, IntPtr.Zero);

            for (int i = 0; i < count; i++)
            {
                User32.SendMessage(window, TB.HIDEBUTTON, i, showButtons);
            }
        }

        private static void TaskManagerMessageBoxButtonPress()
        {
            foreach (var hwnd in WindowsHelper.FindWindowsWithText("Диспетчер задач Windows"))
            {
                var hWndMsgBox = User32.FindWindowEx(hwnd, IntPtr.Zero, "DirectUIHWND", null);
                if (hWndMsgBox == IntPtr.Zero)
                {
                    continue;
                }
                foreach (var ctrlNotifySinkHwnd in WindowsHelper.GetChildWindows(hWndMsgBox))
                {
                    var killPBtn = User32.FindWindowEx(ctrlNotifySinkHwnd, IntPtr.Zero, "Button", null);
                    if (killPBtn == IntPtr.Zero)
                    {
                        continue;
                    }

                    User32.SendMessage(killPBtn, BN.CLICKED, 0, IntPtr.Zero);
                    return;
                }
            }
        }

        private static bool KillProcessFromTaskManager(string processName, IEnumerable<Keys> findProcessByKeys)
        {
            if (!Process.GetProcessesByName("processName").Any())
            {
                return false;
            }

            var tskHwnd = User32.FindWindow("#32770", "Диспетчер задач Windows");
            var pHwnd = User32.FindWindowEx(tskHwnd, IntPtr.Zero, "#32770", "Процессы");
            var pListHwnd = User32.FindWindowEx(pHwnd, IntPtr.Zero, "SysListView32", null);

            foreach (var key in findProcessByKeys)
            {
                User32.PostMessage(pListHwnd, WM.KEYDOWN, Convert.ToInt32(key), IntPtr.Zero);

            }
            Thread.Sleep(200);
            User32.PostMessage(pListHwnd, WM.KEYDOWN, Convert.ToInt32(Keys.Delete), IntPtr.Zero);
            Thread.Sleep(200);
            return true;
        }
    }
}