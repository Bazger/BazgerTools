using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using Bazger.Tools.WinApi;
using NLog;

namespace Bazger.Tools.ObnulAtor
{
    /// <summary>
    /// Helpful websites:
    /// -- MSAA vs UIAutomation: https://stackoverflow.com/questions/27323110/msaa-finds-controls-ui-automation-doesnt
    /// -- MSAA: https://www.codeproject.com/Articles/38906/UI-Automation-Using-Microsoft-Active-Accessibility
    /// -- UIAutomation: https://www.codeproject.com/Articles/141842/Automate-your-UI-using-Microsoft-Automation-Framew
    /// -- Enumerating Notify buttons: https://blogs.msdn.microsoft.com/oldnewthing/20141013-00/?p=43863
    /// -- TestApi for 3ed party UIAutomation lib: https://archive.codeplex.com/?p=testapi
    /// -- Finding opened contex menus example: https://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/89b2ad4a-1b55-40af-a2f8-5469ac946be7/how-to-get-a-list-with-all-context-menu-entries?forum=windowsaccessibilityandautomation 
    /// -- Example for flow of opening tray icon and press on the context menu: https://stackoverflow.com/questions/32354728/programmatically-obtaining-a-windows-applications-tray-menu-items?rq=1
    /// -- Example for Usage of WinAPI and Unsafe function for retriving the BUTTONINFO: https://stackoverflow.com/questions/33652756/how-to-get-the-processes-that-have-systray-icon/33707955
    /// </summary>
    internal static class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static string _kasperskyProcessName;
        private static string _kasperskyServiceName;
        private static string _kasperskyTrayTitle;
        private static string _kasperskyExitButtonTitle;
        private static string _kasperskyResetTrialFileName;
        private static string _kasperskyResetTrialWindowTitle;


        [STAThread]
        private static int Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            Log.Info("---------BazgerTools ObnulAtor v.1.0.0---------");
            try
            {
                LoadConfiguration();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
                return -1;
            }

            if (Process.GetProcessesByName(_kasperskyProcessName).Any())
            {
                Log.Info("Closing KasperskyAV");
                if (!CloseKasperskyAv())
                {
                    return -1;
                }
                Log.Info("KasperskyAV was closed from tray");

                var stopEvent = new ManualResetEvent(false);
                Log.Info("Waiting for KasperskyAV Process and Service stopping");
                while (!stopEvent.WaitOne(200))
                {
                    using (var sc = new ServiceController(_kasperskyServiceName))
                    {
                        if (sc.Status == ServiceControllerStatus.Stopped && !Process.GetProcessesByName(_kasperskyProcessName).Any())
                        {
                            break;
                        }
                    }
                }
                Log.Info("KasperskyAV Process and Service stopped successfully");
            }

            var krt = new KasperskyResetTrialVersion5(_kasperskyResetTrialWindowTitle, _kasperskyResetTrialFileName);
            Log.Info("Openning KRT");
            if (!krt.Open())
            {
                return -1;
            }
            Log.Info("KRT Updating activation");
            if (!krt.ResetActivation())
            {
                Log.Error("KRT can't reset activation");
                return -1;
            }
            Log.Info("KasperskyAV License updated");
            return 0;
        }

        private static void LoadConfiguration()
        {
            _kasperskyProcessName = ConfigurationManager.AppSettings["kasperskyProcessName"] ?? throw new ConfigurationErrorsException("kasperskyProcessName can't be null");
            _kasperskyServiceName = ConfigurationManager.AppSettings["kasperskyServiceName"] ?? throw new ConfigurationErrorsException("kasperskyServiceName can't be null");
            _kasperskyTrayTitle = ConfigurationManager.AppSettings["kasperskyTrayTitle"] ?? throw new ConfigurationErrorsException("kasperskyTrayTitle can't be null");
            _kasperskyExitButtonTitle = ConfigurationManager.AppSettings["kasperskyExitButtonTitle"] ?? throw new ConfigurationErrorsException("kasperskyExitButtonTitle can't be null");
            _kasperskyResetTrialFileName = ConfigurationManager.AppSettings["kasperskyResetTrialFileName"] ?? throw new ConfigurationErrorsException("kasperskyResetTrialFileName can't be null");
            _kasperskyResetTrialWindowTitle = ConfigurationManager.AppSettings["kasperskyResetTrialWindowTitle"] ?? throw new ConfigurationErrorsException("kasperskyResetTrialWindowTitle can't be null");
        }

        private static bool CloseKasperskyAv()
        {
            try
            {
                var trayIcons = AutomationElementHelpers.EnumNotificationIcons();
                foreach (var icon in trayIcons)
                {
                    if (icon.GetCurrentPropertyValue(AutomationElement.NameProperty) is string name &&
                        !name.Contains(_kasperskyTrayTitle))
                    {
                        continue;
                    }
                    var oldPoint = Cursor.Position;
                    var point = new Point((int)icon.GetClickablePoint().X, (int)icon.GetClickablePoint().Y);
                    Cursor.Position = point;
                    MouseHelper.DoMouseRightClick(point.X, point.Y);
                    Cursor.Position = oldPoint;

                    //Find menus and press exit
                    Thread.Sleep(200);
                    var menus = AutomationElementHelpers.GetContextMenuEntriesOnRootMenu();
                    foreach (AutomationElement menuItem in menus)
                    {

                        if (menuItem.GetCurrentPropertyValue(AutomationElement.NameProperty) is string menuItemName &&
                            !menuItemName.Contains(_kasperskyExitButtonTitle))
                        {
                            continue;
                        }
                        menuItem.InvokeButton();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Closing KaspeskyAV was failed");
            }
            return false;
        }
    }
}
