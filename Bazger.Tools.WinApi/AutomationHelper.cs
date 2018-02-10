using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;

namespace Bazger.Tools.WinApi
{
    /// <summary>
    /// More information: 
    /// https://www.codeproject.com/Articles/141842/Automate-your-UI-using-Microsoft-Automation-Framew
    /// https://www.codeproject.com/Articles/38906/UI-Automation-Using-Microsoft-Active-Accessibility
    /// </summary>
    public static class AutomationElementHelpers
    {
        public static AutomationElement FindByName(this AutomationElement root, string name)
        {
            return root.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, name));
        }

        public static AutomationElement FindByClass(this AutomationElement root, string name)
        {
            return root.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ClassNameProperty, name));
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
            invokePattern?.Invoke();
            return invokePattern != null;
        }

        /// <summary>
        /// Example: https://blogs.msdn.microsoft.com/oldnewthing/20141013-00/?p=43863
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<AutomationElement> EnumNotificationIcons()
        {
            //TODO: Add languages support
            foreach (var button in AutomationElement.RootElement.FindByName(
                "Пользовательская область уведомлений"/*"User Promoted Notification Area"*/).EnumChildButtons())
            {
                yield return button;
            }

            //foreach (var button in AutomationElement.RootElement.Find(
            //    "Системная область уведомлений"/*"System Promoted Notification Area"*/).EnumChildButtons())
            //{
            //    yield return button;
            //}

            var chevron = AutomationElement.RootElement.FindByName("NotificationChevron");
            if (chevron != null && chevron.InvokeButton())
            {
                //TODO: Add languages support
                foreach (var button in AutomationElement.RootElement.FindByName(
                    "Область уведомлений переполнения"/*"Overflow Notification Area"*/).EnumChildButtons())
                {
                    yield return button;
                }
            }
        }

        public static void OpenTrayWindow()
        {
            AutomationElement.RootElement.FindByName("NotificationChevron")?.InvokeButton();
        }

        /// <summary>
        /// Took from example: 
        /// https://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/89b2ad4a-1b55-40af-a2f8-5469ac946be7/how-to-get-a-list-with-all-context-menu-entries?forum=windowsaccessibilityandautomation
        /// </summary>
        /// <returns></returns>
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

            return aeCol;
        }
    }
}

