using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using Bazger.Tools.WinApi;

namespace Bazger.Tools.ObnulAtor
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //TODO: Add Configuration for names
            //TODO: Add logging

            //TODO: Add to configuration
            if (Process.GetProcessesByName("avpui").Any())
            {
                if (!CloseKasperskyAv())
                {
                    //TODO: Log here
                    return;
                }
                var stopEvent = new ManualResetEvent(false);

                while (!stopEvent.WaitOne(200))
                {
                    //TODO: Add to configuration
                    using (var sc = new ServiceController("AVP17.0.0"))
                    {
                        //TODO: Add to configuration
                        if (sc.Status == ServiceControllerStatus.Stopped && !Process.GetProcessesByName("avpui").Any())
                        {
                            break;
                        }
                    }
                }
            }
            //TODO: Add to configuration
            var krt = new KasperskyResetTrialVersion5("KRT", "Kaspersky_Reset_Trial_5.1.0.29");
            if (!krt.Open())
            {
                return;
            }
            krt.ResetActivation();
        }

        private static bool CloseKasperskyAv()
        {
            try
            {
                var trayIcons = AutomationElementHelpers.EnumNotificationIcons();
                foreach (var icon in trayIcons)
                {
                    if (icon.GetCurrentPropertyValue(AutomationElement.NameProperty) is string name &&
                        !name.Contains("Kaspersky"))//TODO: Add to configuration
                    {
                        continue;
                    }
                    var point = new Point((int)icon.GetClickablePoint().X, (int)icon.GetClickablePoint().Y);
                    Cursor.Position = point;
                    MouseHelper.DoMouseRightClick(point.X, point.Y);
                    //Find menus and press exit
                    Thread.Sleep(200);
                    var menus = AutomationElementHelpers.GetContextMenuEntriesOnRootMenu();
                    foreach (AutomationElement menuItem in menus)
                    {

                        if (menuItem.GetCurrentPropertyValue(AutomationElement.NameProperty) is string menuItemName &&
                            !menuItemName.Contains("Выход"))//TODO: Add to configuration
                        {
                            continue;
                        }
                        menuItem.InvokeButton();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                //TODO: Log here
            }
            return false;
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
            MouseHelper.DoMouseRightClick(1000, 1000);
            Thread.Sleep(2000);
            AutomationElementHelpers.GetContextMenuEntriesOnRootMenu();
        }
    }
}
