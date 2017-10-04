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
using Bazger.Tools.Clicker.Core;
using Telerik.WinControls;

namespace Bazger.Tools.App.Pages
{
    public partial class ClickerControl : UserControl, IToolControl
    {
        public List<string> Log { get; }


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
            clickerDeleySpin.Value = _clickDelay;

            Log = new List<string>();

            ClickerLog("START/STOP: Try to press this combination: SHIFT+ALT+O");
        }

        public void IntializeControl(MainForm form)
        {
            form.AltShiftOCombinationPressed += ClickerMainProcess;
        }

        /// <summary>
        ///  Button "Start" click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void clickerStartButton_Click(object sender, EventArgs e)
        {
            //ProgramClick.DoMouseClick(Cursor.Position.X, Cursor.Position.Y);
            _clicksCount++;
            UpdateCountLblText();
        }

        /// <summary>
        /// Clicker process
        /// </summary>
        private void ClickerMainProcess(object sender, PageEventArgs e)
        {
            if (e.SelectedPage == null)
            {
                return;
            }

            if (!_isClickerStarted)
            {
                _clicksCount = 0;
                _isClickerStarted = true;
                _clickDelay = (int)clickerDeleySpin.Value;
                clickerDeleySpin.Enabled = false;
                UpdateCountLblText();
                _clickThread = new Thread(ClickerThread);
                _clickThread.Start();
                Log.Add("Clicking Started");
                e.ToolControlsPager.Pages.ToList().ForEach(p =>
                {
                    if (p != e.SelectedPage)
                    {
                        p.Enabled = false;
                    }
                });
            }
            else
            {
                _isClickerStarted = false;
                _allClicksCount += _clicksCount;
                clickerDeleySpin.Enabled = true;
                UpdateCountLblText();
                UpdateAllCountLblText();
                Log.Add("Clicking Stopped");
                e.ToolControlsPager.Pages.ToList().ForEach(p =>
                {
                    if (p != e.SelectedPage)
                    {
                        p.Enabled = true;
                    }
                });
            }
        }

        private void ClickerThread()
        {
            while (_isClickerStarted)
            {
                ProgramClick.DoMouseClick(Cursor.Position.X, Cursor.Position.Y);
                Thread.Sleep(_clickDelay);
                _clicksCount++;
            }
        }

        /// <summary>
        /// Clicker Log
        /// </summary>
        /// <param name="text">String that will be added</param>
        private void ClickerLog(string text)
        {
            //logTxtBox.Text += text + Environment.NewLine;
            //logTxtBox.SelectionStart = logTxtBox.Text.Length;
            //logTxtBox.ScrollToCaret();
        }


        private void UpdateCountLblText()
        {
            clickerCountLbl.Text = CountStr + _clicksCount;
        }


        private void UpdateAllCountLblText()
        {
            clickerAllCountLbl.Text = AllCountStr + _allClicksCount;
        }
    }
}
