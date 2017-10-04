using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Bazger.Tools.App.Pages;
using Bazger.Tools.Clicker.Core;
using Telerik.WinControls;

namespace Bazger.Tools.App
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        private readonly GlobalHotkey _altShiftO;
        private readonly GlobalHotkey _altShiftK;
        private readonly GlobalHotkey _altShiftL;

        public event EventHandler<PageEventArgs> AltShiftOCombinationPressed;
        public event EventHandler<PageEventArgs> AltShiftKCombinationPressed;
        public event EventHandler<PageEventArgs> AltShiftLCombinationPressed;

        private enum HotKeys { AltShiftO, AltShiftK, AltShiftL }
        private List<IToolControl> _viewScreenControls;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Light";
            InitializeComponent();

            //Create HotKeys for the program
            _altShiftO = new GlobalHotkey((int)HotKeys.AltShiftO, Constants.ALT + Constants.SHIFT, Keys.O, this);
            _altShiftK = new GlobalHotkey((int)HotKeys.AltShiftK, Constants.ALT + Constants.SHIFT, Keys.K, this);
            _altShiftL = new GlobalHotkey((int)HotKeys.AltShiftL, Constants.ALT + Constants.SHIFT, Keys.L, this);

            _viewScreenControls = new List<IToolControl> { clickerControl, positionClickerControl, youTubeDownloaderControl};

            //Initialize IToolControls
            _viewScreenControls.ForEach(control => control.IntializeControl(this));

            toolControlsPager_SelectedPageChanged(this, null);
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

            LogOutput(altShiftORegistered
                ? "Hotkey: Alt+Shift+O registered"
                : "Hotkey: Alt+Shift+O failed to registered!");

            LogOutput(altShiftKRegistered
                ? "Hotkey: Alt+Shift+K registered"
                : "Hotkey: Alt+Shift+K failed to registered!");

            LogOutput(altShiftLRegistered
                ? "Hotkey: Alt+Shift+L registered"
                : "Hotkey: Alt+Shift+L failed to registered!");
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

        /// <summary>
        /// Position Clicker Log
        /// </summary>
        /// <param name="text">String that will be added</param>
        private void LogOutput(string text)
        {
            logTxtBox.Text += text + Environment.NewLine;
            logTxtBox.SelectionStart = logTxtBox.Text.Length;
            logTxtBox.ScrollToCaret();
        }

        private void toolControlsPager_SelectedPageChanged(object sender, EventArgs e)
        {
            var control = _viewScreenControls.Find(c => c.ParentPage.TabIndex == toolControlsPager.SelectedPage.TabIndex);
            if (control != null)
            {
                this.MinimumSize = new System.Drawing.Size(((UserControl)control).Size.Width + 13, this.MinimumSize.Height);
            }
        }
    }
}
