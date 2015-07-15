using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace BazgerTools
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private delegate void controller();

        private const string strCount = "Count: ";
        private const string strAllCount = "All Count: ";
        private const string strCircles = "Circles Count: ";

        private GlobalHotkey ghkCkickerTab;
        private GlobalHotkey ghkAddPosClickerTab;
        private GlobalHotkey ghkStartPosClickerTab;

        private Thread clickThread;
        private Thread posClickThread;

        private int clicksCount;
        private int allClicksCount;
        private int circlesCount;

        private int clickDelay = 20;
        private int posClickDelay = 10;

        private volatile Label _countLabel;
        private volatile Label _allCountLabel;
        private volatile Label _circlesLabel;

        private readonly RadGridView _posGridView = new RadGridView();

        private enum HotKeys { StartClickerTab, AddPosClcikerTab, StartPosClickerTab }
        private enum Tabs { Clicker, PositionClicker }

        private bool isClickerStarted;
        private bool isPosClickerStarted;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            clicksCount = 0;
            allClicksCount = 0;
            circlesCount = 0;

            _countLabel = countLabel;
            _allCountLabel = allCountLabel;
            _circlesLabel = circlesLabel;

            _countLabel.Text = strCount + clicksCount;
            _allCountLabel.Text = strAllCount + allClicksCount;
            _circlesLabel.Text = strCircles + circlesCount;

            isClickerStarted = false;
            isPosClickerStarted = false;

            ghkCkickerTab = new GlobalHotkey((int)HotKeys.StartClickerTab, Constants.ALT + Constants.SHIFT, Keys.O, this);
            ghkAddPosClickerTab = new GlobalHotkey((int)HotKeys.AddPosClcikerTab, Constants.ALT + Constants.SHIFT, Keys.K, this);
            ghkStartPosClickerTab = new GlobalHotkey((int)HotKeys.StartPosClickerTab, Constants.ALT + Constants.SHIFT, Keys.L, this);


            _posGridView = posGridView;

            clickerSpinEditor.Value = clickDelay;
            posClickerSpinEditor.Value = posClickDelay;
        }

        /// <summary>
        /// Form Loader
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            ClickerLog("START/STOP: Try to press this combination: SHIFT+ALT+O");
            PositionClickerLog("ADD: Try to press this combination: SHIFT+ALT+K");
            PositionClickerLog("START/STOP: Try to press this combination: SHIFT+ALT+L");

            if (ghkCkickerTab.Register())
                ClickerLog("START/STOP Hotkey: registered.");
            else
                ClickerLog("START/STOP Hotkey: failed to register");

            if (ghkAddPosClickerTab.Register())
                PositionClickerLog("ADD Hotkey: registered.");
            else
                PositionClickerLog("ADD Hotkey: failed to register");

            if (ghkStartPosClickerTab.Register())
                PositionClickerLog("START/STOP Hotkey: registered.");
            else
                PositionClickerLog("START/STOP Hotkey: failed to register");
        }

        /// <summary>
        /// Called when form is closing
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ghkCkickerTab.Unregiser() || !ghkAddPosClickerTab.Unregiser() || !ghkStartPosClickerTab.Unregiser())
                MessageBox.Show("Some Hotkey failed to unregister!");
            isClickerStarted = isPosClickerStarted = false;
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
            {
                switch (m.WParam.ToInt32())
                {
                    case (int)HotKeys.StartClickerTab:
                        Controller((int)Tabs.Clicker, ClickerMainProcess);
                        break;
                    case (int)HotKeys.AddPosClcikerTab:
                        Controller((int)Tabs.PositionClicker, AddData);
                        break;
                    case (int)HotKeys.StartPosClickerTab:
                        Controller((int)Tabs.PositionClicker, PositionClickerMainProcess);
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private bool Controller(int tabId, controller c)
        {
            if (tabId == clickersPageView.SelectedPage.TabIndex)
            {
                c();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the Current cursor, move the cursor's Position, and set its clipping rectangle to the form. 
        /// </summary>
        /// <param name="pos">next movment position</param>
        private void MoveCursor(Point pos)
        {
            Cursor.Position = pos;
        }

        /// <summary>
        /// Call the imported function with the cursor's current position
        /// </summary>
        public void DoMouseClick()
        {
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            mouse_event(Constants.MOUSEEVENTF_LEFTDOWN | Constants.MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
        }

        #region Tab "Clicker"

        /// <summary>
        ///  Button "Start" click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void startClickerButton_Click(object sender, EventArgs e)
        {
            DoMouseClick();
            clicksCount++;
            _countLabel.Text = strCount + clicksCount;
        }

        /// <summary>
        /// Clicker process
        /// </summary>
        public void ClickerMainProcess()
        {
            if (!isClickerStarted)
            {
                clicksCount = 0;
                isClickerStarted = true;
                clickDelay = (int)clickerSpinEditor.Value;
                clickerSpinEditor.Enabled = false;
                _countLabel.Text = strCount + clicksCount;
                clickThread = new Thread(ClickerThread);
                clickThread.Start();
                ClickerLog("Clicking Started");
            }
            else
            {
                isClickerStarted = false;
                allClicksCount += clicksCount;
                clickerSpinEditor.Enabled = true;
                _countLabel.Text = strCount + clicksCount;
                _allCountLabel.Text = strAllCount + allClicksCount;
                ClickerLog("Clicking Stopped");
            }
        }

        public void ClickerThread()
        {
            while (isClickerStarted)
            {
                DoMouseClick();
                Thread.Sleep(clickDelay);
                clicksCount++;
            }
        }

        /// <summary>
        /// Clicker Log
        /// </summary>
        /// <param name="text">String that will be added</param>
        private void ClickerLog(string text)
        {
            clickerLog.Text += text + Environment.NewLine;
            clickerLog.SelectionStart = clickerLog.Text.Length;
            clickerLog.ScrollToCaret();

        }

        #endregion

        #region Tab "Position Clicker"

        /// <summary>
        /// Position Clicker process
        /// </summary>
        public void PositionClickerMainProcess()
        {
            if (!isPosClickerStarted)
            {
                isPosClickerStarted = true;
                posClickerPanel.Enabled = false;
                posClickDelay = (int)posClickerSpinEditor.Value;
                posClickThread = new Thread(PositionClickerThread);
                posClickThread.Start();
                PositionClickerLog("Clicking Started");
            }
            else
            {
                isPosClickerStarted = false;
                posClickerPanel.Enabled = true;
                PositionClickerLog("Clicking Stopped");
                _circlesLabel.Text = strCircles + circlesCount;
            }
        }

        public void PositionClickerThread()
        {
            while (isPosClickerStarted)
            {
                for (int i = 0; i < _posGridView.RowCount && isPosClickerStarted; i++)
                {
                    string[] resultsArray = posGridView.Rows[i].Cells[0].Value.ToString().Split(',');
                    MoveCursor(new Point(Convert.ToInt32(resultsArray[0]), Convert.ToInt32(resultsArray[1])));
                    DoMouseClick();
                    Thread.Sleep(posClickDelay);
                }
                circlesCount++;
            }
        }

        /// <summary>
        /// Adding position info to GridView
        /// </summary>
        private void AddData()
        {
            if (!isPosClickerStarted)
                if (Cursor.Current != null)
                {
                    Cursor = new Cursor(Cursor.Current.Handle);
                    _posGridView.Rows.Add(Cursor.Position.X + "," + Cursor.Position.Y);
                    PositionClickerLog("Position (" + Cursor.Position.X + ", " + Cursor.Position.Y + ") was added");
                }
                else
                    PositionClickerLog("Can't add cursor position!");
            else
                PositionClickerLog("Clicking thread is working! Stop it for adding a new position");

        }

        /// <summary>
        /// Removing data on current position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeButton_Click(object sender, EventArgs e)
        {
            if (_posGridView.Rows.Count > 0)
                try
                {
                    int selectedIndex = _posGridView.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        _posGridView.Rows.RemoveAt(selectedIndex);
                        PositionClickerLog("Postition removed");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    PositionClickerLog("Unable to remove selected row at this time");
                }
            else
                PositionClickerLog("Select the row");

        }

        /// <summary>
        /// Removing all data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAllButton_Click(object sender, EventArgs e)
        {
            if (_posGridView.Rows.Count > 0)
            {
                while (_posGridView.Rows.Count > 0)
                {
                    _posGridView.Rows.RemoveAt(_posGridView.Rows.Count - 1);
                }
                PositionClickerLog("All data was removed");
            }
        }

        /// <summary>
        /// Button "Move Up" event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void moveUpButton_Click(object sender, EventArgs e)
        {
            MoveUpDown(-1);
        }

        /// <summary>
        /// Button "Move Down" event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void moveDownButton_Click(object sender, EventArgs e)
        {
            MoveUpDown(1);
        }

        /// <summary>
        /// Moving data controller
        /// </summary>
        /// <param name="jumps">Jumps to new position</param>
        private void MoveUpDown(int jumps)
        {
            if (_posGridView.Rows.Count > 0)
            {
                int selectedIndex = _posGridView.CurrentCell.RowIndex;
                if (selectedIndex > -1)
                {
                    if (selectedIndex + jumps >= 0 && selectedIndex + jumps < _posGridView.Rows.Count)
                    {
                        _posGridView.Rows[selectedIndex].IsSelected = false;
                        _posGridView.Rows.Move(selectedIndex, selectedIndex + jumps);
                        _posGridView.Rows[selectedIndex + jumps].IsSelected = true;
                        PositionClickerLog("Postition moved UP/DOWN");
                    }

                }
            }
        }

        /// <summary>
        /// Position Clicker Log
        /// </summary>
        /// <param name="text">String that will be added</param>
        private void PositionClickerLog(string text)
        {
            posClickerLog.Text += text + Environment.NewLine;
            posClickerLog.SelectionStart = posClickerLog.Text.Length;
            posClickerLog.ScrollToCaret();
        }

        #endregion

        private void addButton_Click(object sender, EventArgs e)
        {

        }
    }
}
