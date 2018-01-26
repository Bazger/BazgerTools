using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bazger.Tools.App.NLog;
using Bazger.Tools.App.Pages;
using Bazger.Tools.App.State;
using Bazger.Tools.App.Utils;
using Bazger.Tools.Clicker.Core;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App
{
    public partial class MainForm : RadForm
    {
        private const string StateFilePath = "state.json";
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
        private bool _formCanBeClosed;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitizlizeContextMenus();

            _viewScreenControls = new List<IToolControl> { clickerControl, positionClickerControl, youTubeDownloaderControl };
            SetParentPagesToContols();

            //Create HotKeys for the program
            _altShiftO = new GlobalHotkey((int)HotKeys.AltShiftO, Constants.ALT + Constants.SHIFT, Keys.O, this);
            _altShiftK = new GlobalHotkey((int)HotKeys.AltShiftK, Constants.ALT + Constants.SHIFT, Keys.K, this);
            _altShiftL = new GlobalHotkey((int)HotKeys.AltShiftL, Constants.ALT + Constants.SHIFT, Keys.L, this);

            //Set loggers
            _viewScreenControls.ForEach(c => CreatePageLogger(c.LoggerName, c.LoggerName));
            //Set postion for page controls
            _viewScreenControls.ForEach(c =>
            {
                var uc = (UserControl)c;
                uc.Left = (c.ParentPage.Width - uc.Width) / 2;
                uc.Top = (c.ParentPage.Height - uc.Height) / 2;
            });

            ToolControlsPager_SelectedPageChanged(this, null);
        }

        private void InitizlizeContextMenus()
        {
            clearLogs.Click += ClearLogs_Click;
            clearAllLogs.Click += ClearAllLogs_Click;
            logTxtBox.TextBoxElement.TextBoxItem.HostedControl.ContextMenu = new ContextMenu();
            cntxMenuManager.SetRadContextMenu(logTxtBox.TextBoxElement.TextBoxItem.HostedControl, logTxtBoxCntxMenu);
        }

        private void SetParentPagesToContols()
        {
            _viewScreenControls.ForEach(c =>
            {
                for (var i = 0; i < toolControlsPager.Controls.Count; i++)
                {
                    var page = toolControlsPager.Controls[i];
                    if (!page.Controls.Contains((UserControl)c))
                    {
                        continue;
                    }
                    c.ParentPage = (RadPageViewPage)page;
                }
            });
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

            //Loading states of page controls
            try
            {
                var formState = File.Exists(StateFilePath)
                    ? SerDeHelper.DeserializeJsonFile<FormState>(StateFilePath,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                    : null;
                if (formState != null)
                {
                    _viewScreenControls.ForEach(c =>
                    {
                        if (!(c is IControlStateChanger stateControl))
                        {
                            return;
                        }
                        if (formState.ControlStates.ContainsKey(c.LoggerName))
                        {
                            stateControl.LoadState(formState.ControlStates[c.LoggerName]);
                        }

                    });
                    Log.Info("Successfully loaded state for controls");
                }
            }
            catch (Exception)
            {
                Log.Error("Can't load states for controls");
            }
        }

        /// <summary>
        /// Called when form is closing
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_formCanBeClosed)
            {
                e.Cancel = true;

                //var waitingScreen = new WaitingScreen { StartPosition = FormStartPosition.Manual };
                //waitingScreen.Location = new Point(this.Location.X + (this.Width - waitingScreen.Width) / 2, this.Location.Y + (this.Height - waitingScreen.Height) / 2);
                //waitingScreen.Show(this);

                new Task(() =>
                {
                    _viewScreenControls.ForEach(c => c.OnFormClosing(sender, e));
                    var formSate = new FormState();
                    _viewScreenControls.ForEach(c =>
                    {
                        var stateControl = c as IControlStateChanger;
                        var state = stateControl?.SaveState();
                        if (state != null)
                        {
                            formSate.ControlStates.Add(c.LoggerName, state);
                        }
                    });
                    SerDeHelper.SerializeToJsonFile(formSate, StateFilePath,
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                    _formCanBeClosed = true;
                    //FormHelper.ControlInvoker(waitingScreen, control => control.Close());                 
                    FormHelper.ControlInvoker(this, control => this.Close());
                }).Start();
            }
            else
            {
                if (!_altShiftK.Unregiser() || !_altShiftL.Unregiser() || !_altShiftO.Unregiser())
                {
                    MessageBox.Show("Some Hotkey failed to unregister!");
                }
            }
        }


        public void IsPageSwitichingEnabled(bool isEnabled)
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

        private void SetMinimuFormSize(IToolControl control)
        {
            var backstagePanelWidth = 0;
            if (toolControlsPager.ViewElement is RadPageViewBackstageElement backstageElement)
            {
                backstagePanelWidth = backstageElement.ItemAreaWidth;
            }
            var controlSize = ((UserControl)control)?.Size.Width ?? _viewScreenControls.Max(c => ((UserControl)c).Size.Width);
            MinimumSize = new System.Drawing.Size(controlSize + backstagePanelWidth + 15, MinimumSize.Height);
        }

        private IToolControl GetSelectedPageControl()
        {
            return _viewScreenControls.FirstOrDefault(c => c.ParentPage.TabIndex == toolControlsPager.SelectedPage.TabIndex);
        }

        private void ToolControlsPager_SelectedPageChanged(object sender, EventArgs e)
        {
            var control = GetSelectedPageControl();
            if (control == null)
            {
                return;
            }
            SetMinimuFormSize(control);

            if (_currentPageLogs != null)
            {
                _currentPageLogs.CollectionChanged -= UpdateLogTexBox;
            }
            _currentPageLogs = ((ObservableMemoryTarget)LogManager.Configuration.FindTargetByName(control.LoggerName)).Logs;

            var text = string.Empty;
            _currentPageLogs.ToList().ForEach(l => text += l + Environment.NewLine);
            logTxtBox.Text = text;
            _currentPageLogs.CollectionChanged += UpdateLogTexBox;
        }

        private void UpdateLogTexBox(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (string newItem in e.NewItems)
                    {
                        FormHelper.ControlInvoker(logTxtBox, control => control.AppendText(newItem + Environment.NewLine));
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    FormHelper.ControlInvoker(logTxtBox, control => control.Clear());
                    break;
            }
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
            config.LoggingRules.First(r => r.LoggerNamePattern == GetType().FullName)?.Targets.Add(memoryTarget);
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

        public Target GetLogTarget(string targetName)
        {
            var config = Log.Factory.Configuration;
            var target = LogManager.Configuration.FindTargetByName(targetName);
            return target;
        }

        private void ClearLogs_Click(object sender, EventArgs e)
        {
            var control = GetSelectedPageControl();
            if (control == null)
            {
                return;
            }
            var target = (ObservableMemoryTarget)GetLogTarget(control.LoggerName);
            target?.Logs.Clear();
        }

        private void ClearAllLogs_Click(object sender, EventArgs e)
        {
            _viewScreenControls.ForEach(c =>
                {
                    var target = (ObservableMemoryTarget)GetLogTarget(c.LoggerName);
                    target?.Logs.Clear();
                }
            );
        }
    }
}
