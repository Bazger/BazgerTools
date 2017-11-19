using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Bazger.Tools.App.NLog;
using Bazger.Tools.App.Pages;
using Bazger.Tools.App.Utils;
using Bazger.Tools.Clicker.Core;
using NLog;
using NLog.Config;
using NLog.Filters;
using NLog.Targets;
using NLog.Windows.Forms;
using Telerik.WinControls;

namespace Bazger.Tools.App
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private ObservableCollection<string> _currentPageLogs;

        private readonly GlobalHotkey _altShiftO;
        private readonly GlobalHotkey _altShiftK;
        private readonly GlobalHotkey _altShiftL;

        public event EventHandler<PageEventArgs> AltShiftOCombinationPressed;
        public event EventHandler<PageEventArgs> AltShiftKCombinationPressed;
        public event EventHandler<PageEventArgs> AltShiftLCombinationPressed;

        private enum HotKeys { AltShiftO, AltShiftK, AltShiftL }
        private readonly List<IToolControl> _viewScreenControls;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
            InitializeComponent();

            //Create HotKeys for the program
            _altShiftO = new GlobalHotkey((int)HotKeys.AltShiftO, Constants.ALT + Constants.SHIFT, Keys.O, this);
            _altShiftK = new GlobalHotkey((int)HotKeys.AltShiftK, Constants.ALT + Constants.SHIFT, Keys.K, this);
            _altShiftL = new GlobalHotkey((int)HotKeys.AltShiftL, Constants.ALT + Constants.SHIFT, Keys.L, this);

            _viewScreenControls = new List<IToolControl> { clickerControl, positionClickerControl, youTubeDownloaderControl };
            //Set loggers
            _viewScreenControls.ForEach(c => CreatePageLogger(c.Title, c.Title));

            toolControlsPager_SelectedPageChanged(this, null);

            FormSerialisor.Deserialise(this, Application.StartupPath + @"\serialise.xml");
        }

        /// <summary>
        /// Form Loader
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            var altShiftORegistered = _altShiftO.Register();
            var altShiftKRegistered = _altShiftK.Register();
            var altShiftLRegistered = _altShiftL.Register();

            if (altShiftORegistered)
            {
                Log.Info("Hotkey: Alt+Shift+O registered");
            }
            else
            {
                Log.Error("Hotkey: Alt+Shift+O failed to registered!");
            }

            if (altShiftKRegistered)
            {
                Log.Info("Hotkey: Alt+Shift+K registered");
            }
            else
            {
                Log.Error("Hotkey: Alt+Shift+K failed to registered!");
            }

            if (altShiftLRegistered)
            {
                Log.Info("Hotkey: Alt+Shift+L registered");
            }
            else
            {
                Log.Error("Hotkey: Alt+Shift+L failed to registered!");
            }

            //Initialize IToolControls
            _viewScreenControls.ForEach(control => control.IntializeControl(this));

        }

        /// <summary>
        /// Called when form is closing
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_altShiftK.Unregiser() || !_altShiftL.Unregiser() || !_altShiftO.Unregiser())
            {
                MessageBox.Show("Some Hotkey failed to unregister!");
            }
            _viewScreenControls.ForEach(c => c.OnFormClosing(sender, e));
            FormSerialisor.Serialise(this, Application.StartupPath + @"\serialise.xml");
        }


        public void ChangeEnabledStateOfNotSelectedTabs(bool isEnabled)
        {
            toolControlsPager.Pages.ToList().ForEach(p =>
            {
                if (p != toolControlsPager.SelectedPage)
                {
                    p.Enabled = isEnabled;
                }
            });
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
            {
                switch (m.WParam.ToInt32())
                {
                    case (int)HotKeys.AltShiftO:
                        AltShiftOCombinationPressed?.Invoke(this, new PageEventArgs(toolControlsPager));
                        break;
                    case (int)HotKeys.AltShiftK:
                        AltShiftKCombinationPressed?.Invoke(this, new PageEventArgs(toolControlsPager));
                        break;
                    case (int)HotKeys.AltShiftL:
                        AltShiftLCombinationPressed?.Invoke(this, new PageEventArgs(toolControlsPager));
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void toolControlsPager_SelectedPageChanged(object sender, EventArgs e)
        {
            var control = _viewScreenControls.Find(c => c.ParentPage.TabIndex == toolControlsPager.SelectedPage.TabIndex);
            if (control == null)
            {
                return;
            }
            this.MinimumSize = new System.Drawing.Size(((UserControl)control).Size.Width + 13, this.MinimumSize.Height);

            if (_currentPageLogs != null)
            {
                _currentPageLogs.CollectionChanged -= UpdateLogTexBox;
            }
            _currentPageLogs = ((ObservableMemoryTarget)LogManager.Configuration.FindTargetByName(control.Title)).Logs;

            var text = string.Empty;
            _currentPageLogs.ToList().ForEach(l => text += l + Environment.NewLine);
            logTxtBox.Text = text;
            _currentPageLogs.CollectionChanged += UpdateLogTexBox;

            //logTxtBox.SelectionStart = logTxtBox.TextLength;
            //SetScrollPos(logTxtBox.Handle, 1, 50, true);
            //int b = SetScrollPos(logTxtBox.Handle, 1, 200, true);
            //Debug.WriteLine($"scroll-{b}");
        }

        unsafe private void UpdateLogTexBox(object sender, NotifyCollectionChangedEventArgs e)
        {
            var lockStatus = LockWindow(logTxtBox.Handle);
            Debug.WriteLine($"lockStatus-{lockStatus}");

            foreach (string newItem in e.NewItems)
            {
                logTxtBox.AppendText(newItem + Environment.NewLine);

                int scrollPos = LogTextBoxVScrollPos;
                int pos = logTxtBox.SelectionStart;
                int len = logTxtBox.SelectionLength;
                logTxtBox.Text += newItem + Environment.NewLine;
                logTxtBox.SelectionStart = pos;
                logTxtBox.SelectionLength = len;
                LogTextBoxVScrollPos = scrollPos;

                Debug.WriteLine($"scroll-{LogTextBoxVScrollPos}");
                var currentPos = LogTextBoxVScrollPos;
                if (logTxtBox.Lines.Length - 1 > logTxtBox.GetLineFromCharIndex(logTxtBox.SelectionStart))
                {
                    logTxtBox.Text += newItem + Environment.NewLine;
                    int minScroll;
                    int maxScroll;
                    GetScrollRange(this.logTxtBox.Handle, Orientation.Vertical, out minScroll, out maxScroll);
                    Debug.WriteLine($"Min-{minScroll}");
                    Debug.WriteLine($"Max-{maxScroll}");
                    //SetScrollPos(logTxtBox.Handle, Orientation.Vertical, currentPos, true);
                }
                else
                {
                    logTxtBox.AppendText(newItem + Environment.NewLine);
                }
            }
            lockStatus = LockWindow(IntPtr.Zero);
            Debug.WriteLine($"lockStatus-{lockStatus}");
        }

        int LogTextBoxVScrollPos
        {
            get { return GetScrollPos(logTxtBox.Handle, Orientation.Vertical); }
            set { SetScrollPos(logTxtBox.Handle, Orientation.Vertical, value, true); }
        }

        private void CreatePageLogger(string loggerName, string targetName)
        {
            var config = Log.Factory.Configuration;

            var memoryTarget = new ObservableMemoryTarget()
            {
                Layout = config.Variables["Layout"],
                Name = targetName
            };
            config.AddTarget(memoryTarget);

            var rule = new LoggingRule(loggerName, LogLevel.Info, memoryTarget);
            //Write main window log to all targets
            config.LoggingRules.First(r => r.LoggerNamePattern == this.GetType().FullName)?.Targets.Add(memoryTarget);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
        }

        public void AddRuleToPageLoggerTarget(string loggerName, LogLevel logLevel, string targetName)
        {
            var config = Log.Factory.Configuration;

            //Searching the page logger target
            var target = LogManager.Configuration.FindTargetByName(targetName);
            if (target == null)
            {
                return;
            }
            var rule = new LoggingRule(loggerName, logLevel, target);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
        }

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, Orientation nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetScrollPos(IntPtr hWnd, Orientation nBar);

        [DllImport("user32.dll")]
        static extern bool GetScrollRange(IntPtr hWnd, Orientation nBar, out int lpMinPos, out int lpMaxPos);

        [DllImport("user32.dll")]
        static extern int ScrollWindowEx(IntPtr hWnd, int dx, int dy, IntPtr prcScroll, IntPtr prcClip, IntPtr hrgnUpdate, IntPtr prcUpdate, uint flags);

        [DllImport("user32.dll", EntryPoint = "LockWindowUpdate", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LockWindow(IntPtr hWnd);
    }
}
