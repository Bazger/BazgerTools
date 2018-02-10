using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bazger.Tools.App.State;
using Bazger.Tools.WinApi;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Pages
{
    public partial class ClickerControl : UserControl, IToolControl
    {
        private MainForm _mainForm;
        public string LoggerName => this.GetType().FullName;
        public RadPageViewPage ParentPage { get; set; }
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private Thread _clickThread;
        private const string CountStr = "Count: ";
        private const string AllCountStr = "All Count: ";

        private int _clicksCount;
        private int _allClicksCount;
        private bool _isClickerStarted;
        private int _clickDelay;


        public ClickerControl()
        {
            InitializeComponent();

            _isClickerStarted = false;
            _clicksCount = 0;
            _allClicksCount = 0;

            UpdateCountLblText();
            UpdateAllCountLblText();

            _clickDelay = 20;
            deleySpin.Value = _clickDelay;
        }

        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;
            mainForm.AltShiftOCombinationPressed += ClickerMainProcess;
            _log.Info("START/STOP: Try to press this combination: SHIFT+ALT+O");
        }

        public void OnFormClosing(object sender, FormClosingEventArgs e)
        {
        }

        /// <summary>
        ///  Button "Start" click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void ClickerStartButton_Click(object sender, EventArgs e)
        {
            _clicksCount++;
            UpdateCountLblText();
        }

        /// <summary>
        /// Clicker process
        /// </summary>
        private void ClickerMainProcess(object sender, PageEventArgs e)
        {
            if (ParentPage.TabIndex != e.SelectedPage.TabIndex)
            {
                return;
            }

            if (!_isClickerStarted)
            {
                _clicksCount = 0;
                _isClickerStarted = true;
                _clickDelay = (int)deleySpin.Value;
                deleySpin.Enabled = false;
                UpdateCountLblText();
                _clickThread = new Thread(ClickerThread);
                _clickThread.Start();
                _log.Info("Clicking Started");
                _mainForm.IsPageSwitichingEnabled(false);
            }
            else
            {
                _isClickerStarted = false;
                _allClicksCount += _clicksCount;
                deleySpin.Enabled = true;
                UpdateCountLblText();
                UpdateAllCountLblText();
                _log.Info("Clicking Stopped");
                _mainForm.IsPageSwitichingEnabled(true);
            }
        }

        private void ClickerThread()
        {
            while (_isClickerStarted)
            {
                MouseHelper.DoMouseLeftClick(Cursor.Position.X, Cursor.Position.Y);
                Thread.Sleep(_clickDelay);
                _clicksCount++;
            }
        }

        private void UpdateCountLblText()
        {
            countLbl.Text = CountStr + _clicksCount;
        }


        private void UpdateAllCountLblText()
        {
            allCountLbl.Text = AllCountStr + _allClicksCount;
        }
    }
}
