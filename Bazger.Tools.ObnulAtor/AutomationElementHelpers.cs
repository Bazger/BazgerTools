using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Automation;
using System.Linq;
using Bazger.Tools.ObnulAtor.Utils;


namespace Bazger.Tools.ObnulAtor
{
    static class AutomationElementHelpers
    {
        public static AutomationElement Find(this AutomationElement root, string name)
        {
            return root.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, name));
        }

        public static IEnumerable<AutomationElement>
            EnumChildButtons(this AutomationElement parent)
        {
            return parent == null
                ? Enumerable.Empty<AutomationElement>()
                : parent.FindAll(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty,
                        ControlType.Button)).Cast<AutomationElement>();
        }

        public static bool InvokeButton(this AutomationElement button)
        {
            var invokePattern = button.GetCurrentPattern(InvokePattern.Pattern)
                as InvokePattern;
            if (invokePattern != null)
            {
                invokePattern.Invoke();
            }
            return invokePattern != null;
        }

        public static IEnumerable<AutomationElement> EnumNotificationIcons()
        {
            foreach (var button in AutomationElement.RootElement.Find(
                "Пользовательская область уведомлений").EnumChildButtons())
            {
                yield return button;
            }

            //foreach (var button in AutomationElement.RootElement.Find(
            //    "Системная область уведомлений").EnumChildButtons())
            //{
            //    yield return button;
            //}

            //OpenTray();
            ////var chevron = AutomationElement.RootElement.Find("NotificationChevron");
            ////if (chevron != null && chevron.InvokeButton())
            ////{
            //    foreach (var button in AutomationElement.RootElement.Find(
            //        "Область уведомлений переполнения").EnumChildButtons())
            //    {
            //        yield return button;
            //    }
            ////}
        }

        private static IntPtr OpenTray()
        {
            var hWndTray = User32.FindWindow("Shell_TrayWnd", null);
            if (hWndTray == IntPtr.Zero) { return IntPtr.Zero; }
            hWndTray = User32.FindWindowEx(hWndTray, IntPtr.Zero, "TrayNotifyWnd", null);
            if (hWndTray == IntPtr.Zero) { return IntPtr.Zero; }
            hWndTray = User32.FindWindowEx(hWndTray, IntPtr.Zero, "Button", null);
            User32.SendMessage(hWndTray, User32.BN_CLICKED, 0, 0);
            return hWndTray;
        }

        //public static void A()
        //{
        //    var aeContextMenuContainer = AutomationElement.RootElement;
        //    var cond = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Menu);

        //    //get the menu
        //    var aeContextMenu = aeContextMenuContainer.FindFirst(TreeScope.Children, cond);

        //    //get the children
        //    AutomationElementCollection aeCol = aeContextMenu.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));

        //    foreach (AutomationElement aeItem in aeCol)
        //    {
        //        //you could do somrthing with a particular child, like expand. 
        //        //In which case you could refactor this example to be recursive 
        //        Debug.WriteLine(aeItem.Current.Name);
        //    }
        //}

        public static AutomationElementCollection GetContextMenuEntriesOnRootMenu()
        {
            //At this point your context menu is open. 
            //Root element is the Visual Studio Window
            AutomationElement aeContextMenuContainer = AutomationElement.RootElement;
            Condition cond = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Menu);
            //get the menu
            var aeContextMenu = aeContextMenuContainer.FindFirst(TreeScope.Children, cond);
            //get the children
            AutomationElementCollection aeCol = aeContextMenu.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem));
            //Assert.AreEqual( entries.Count, aeCol.Count, "There are not the correct count of menu entries in the list" );

            foreach (AutomationElement aeItem in aeCol)
            {
                //you could do somrthing with a particular child, like expand. 
                //In which case you could refactor this example to be recursive 
                Debug.WriteLine(aeItem.Current.Name);
                if (aeItem.Current.Name == "Выход")
                {
                    aeItem.InvokeButton();
                }
            }

            return aeCol;
        }
    }
}

