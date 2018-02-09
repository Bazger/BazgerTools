using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using Bazger.Tools.ObnulAtor.Utils;
using Bazger.Tools.WinApi;

namespace Bazger.Tools.ObnulAtor
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //TODO: Add close processes of AV
            //TODO: DONE Add loading from licence (We have no licence)
            //TODO: Add logging
            //CloseAntiviruses();
            //OpenTray();
            //ShowTaskbarButtons();
            Automation();

            //var krt = new KasperskyResetTrialVersion5("KRT", "Kaspersky_Reset_Trial_5.1.0.29");
            //if (!krt.Open())
            //{
            //    return;
            //}
            //krt.ResetActivation();
        }


        private static void Automation()
        {
            //foreach (var icon in AutomationElementHelpers.EnumNotificationIcons())
            //{
            //    var name = icon.GetCurrentPropertyValue(AutomationElement.NameProperty)
            //        as string;
            //    System.Console.WriteLine(name);
            //    System.Console.WriteLine("---");
            //}
            var element = AutomationElementHelpers.EnumNotificationIcons().First();
            Cursor.Position = new Point((int)element.GetClickablePoint().X, (int)element.GetClickablePoint().Y);
            ProgramClick.DoMouseRightClick(1000, 1000);
            Thread.Sleep(2000);
            AutomationElementHelpers.GetContextMenuEntriesOnRootMenu();
        }

        // add references WindowsBase, UIAutomationClient, UIAutomationTypes
        // using System.Windows.Automation; 
        // using System.Windows;
        public static void RightClick(AutomationElement element)
        {
            var point = element.GetClickablePoint();
            object processId = element.GetCurrentPropertyValue(AutomationElement.ProcessIdProperty);
            AutomationElement window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ProcessIdProperty, processId));
            Int32 hWnd = window.Current.NativeWindowHandle;
            double x = point.X;
            double y = point.Y;
            Int32 value = ((int)x) << 16 + (int)y;
            User32.SendMessage(new IntPtr(hWnd), User32.WM_LBUTTONDOWN, IntPtr.Zero, new IntPtr(value));
            User32.SendMessage(new IntPtr(hWnd), User32.WM_LBUTTONUP, IntPtr.Zero, new IntPtr(value));
        }

        private static IntPtr GetNotifyWindow()
        {
            var hWndTray = User32.FindWindow("Shell_TrayWnd", null);
            if (hWndTray == IntPtr.Zero) { return IntPtr.Zero; }
            hWndTray = User32.FindWindowEx(hWndTray, IntPtr.Zero, "TrayNotifyWnd", null);
            if (hWndTray == IntPtr.Zero) { return IntPtr.Zero; }
            hWndTray = User32.FindWindowEx(hWndTray, IntPtr.Zero, "SysPager", null);
            if (hWndTray == IntPtr.Zero) { return IntPtr.Zero; }
            hWndTray = User32.FindWindowEx(hWndTray, IntPtr.Zero, "ToolbarWindow32", null);
            return hWndTray;
        }

        public static void DeleteAllTaskbarButtons()
        {
            IntPtr window = GetNotifyWindow();
            int count = (int)User32.SendMessage(window, User32.TB_BUTTONCOUNT, IntPtr.Zero, IntPtr.Zero);

            for (int i = 0; i < count; i++)
            {
                User32.SendMessage(window, User32.TB_DELETEBUTTON, i, 0);
            }
        }


        public static void ShowTaskbarButtons(bool showButtons = false)
        {
            IntPtr window = GetNotifyWindow();
            int count = (int)User32.SendMessage(window, User32.TB_BUTTONCOUNT, IntPtr.Zero, IntPtr.Zero);

            for (int i = 0; i < count; i++)
            {
                User32.SendMessage(window, User32.WM_LBUTTONDOWN, i, IntPtr.Zero);
                User32.SendMessage(window, User32.WM_LBUTTONUP, i, IntPtr.Zero);
                User32.SendMessage(window, User32.BN_CLICKED, i, IntPtr.Zero);
            }
        }


        private static IntPtr PressOnTraySettings()
        {
            var hWnd = User32.GetDesktopWindow();
            var notifyAreaHWnd = User32.FindWindowEx(hWnd, IntPtr.Zero, "NotifyIconOverflowWindow", null);
            var iconsAreaHWnd = User32.FindWindowEx(notifyAreaHWnd, IntPtr.Zero, "ToolbarWindow32", null);
            return hWnd;
        }

        private static void CloseAntiviruses()
        {
            //TODO: Reopen taskmgr
            Process.Start("taskmgr");
            Thread.Sleep(500);

            while (CloseKasperskyAv())
            {
                Thread.Sleep(300);
            }

            //TODO: Close taskmgr
        }

        private static bool CloseKasperskyAv()
        {
            if (!Process.GetProcessesByName("avpui").Any())
            {
                return false;
            }

            var tskHwnd = User32.FindWindow("#32770", "Диспетчер задач Windows");
            var pHwnd = User32.FindWindowEx(tskHwnd, IntPtr.Zero, "#32770", "Процессы");
            var pListHwnd = User32.FindWindowEx(pHwnd, IntPtr.Zero, "SysListView32", null);

            User32.PostMessage(pListHwnd, User32.WM_KEYDOWN, Convert.ToInt32(Keys.A), IntPtr.Zero);
            User32.PostMessage(pListHwnd, User32.WM_KEYDOWN, Convert.ToInt32(Keys.V), IntPtr.Zero);
            User32.PostMessage(pListHwnd, User32.WM_KEYDOWN, Convert.ToInt32(Keys.P), IntPtr.Zero);
            User32.PostMessage(pListHwnd, User32.WM_KEYDOWN, Convert.ToInt32(Keys.U), IntPtr.Zero);
            User32.PostMessage(pListHwnd, User32.WM_KEYDOWN, Convert.ToInt32(Keys.I), IntPtr.Zero);
            Thread.Sleep(200);
            User32.PostMessage(pListHwnd, User32.WM_KEYDOWN, Convert.ToInt32(Keys.Delete), IntPtr.Zero);
            Thread.Sleep(200);
            TaskMgrKillProcessButtonPress();
            return true;
        }

        private static void TaskMgrKillProcessButtonPress()
        {
            foreach (var hwnd in WindowsHelper.FindWindowsWithText("Диспетчер задач Windows"))
            {
                var hWndMsgBox = User32.FindWindowEx(hwnd, IntPtr.Zero, "DirectUIHWND", null);
                if (hWndMsgBox == IntPtr.Zero)
                {
                    continue;
                }
                foreach (var ctrlNotifySinkHwnd in WindowsHelper.FindChildWindows(hWndMsgBox))
                {
                    var killPBtn = User32.FindWindowEx(ctrlNotifySinkHwnd, IntPtr.Zero, "Button", null);
                    if (killPBtn == IntPtr.Zero)
                    {
                        continue;
                    }

                    User32.SendMessage(killPBtn, User32.BN_CLICKED, 0, IntPtr.Zero);
                    return;
                }
            }
        }

    }
}
