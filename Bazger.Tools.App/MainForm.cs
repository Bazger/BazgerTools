using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Bazger.Tools.App.Pages;
using Bazger.Tools.Clicker.Core;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);


        private const string StrCircles = "Circles Count: ";

        private readonly GlobalHotkey _altShiftO;
        private readonly GlobalHotkey _altShiftK;
        private readonly GlobalHotkey _altShiftL;

        public event EventHandler<PageEventArgs> AltShiftOCombinationPressed;
        public event EventHandler<PageEventArgs> AltShiftKCombinationPressed;
        public event EventHandler<PageEventArgs> AltShiftLCombinationPressed;

        private Thread _posClickThread;

        private int _circlesCount;
        private int _posClickDelay = 10;

        private enum HotKeys { AltShiftO, AltShiftK, AltShiftL }

        private bool _isPosClickerStarted;

        private List<IToolControl> _viewScreenControls;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
            InitializeComponent();

            _circlesCount = 0;
            posClickerCirclesLbl.Text = StrCircles + _circlesCount;

            _isPosClickerStarted = false;       
            posClickerSpin.Value = _posClickDelay;

            //Create HotKeys for the program
            _altShiftO = new GlobalHotkey((int)HotKeys.AltShiftO, Constants.ALT + Constants.SHIFT, Keys.O, this);
            _altShiftK = new GlobalHotkey((int)HotKeys.AltShiftK, Constants.ALT + Constants.SHIFT, Keys.K, this);
            _altShiftL = new GlobalHotkey((int)HotKeys.AltShiftL, Constants.ALT + Constants.SHIFT, Keys.L, this);

            _viewScreenControls = new List<IToolControl> { clickerControl };

            //Initialize IToolControls
            _viewScreenControls.ForEach(control => control.IntializeControl(this));
        }

        /// <summary>
        /// Form Loader
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            PositionClickerLog("ADD: Try to press this combination: SHIFT+ALT+K");
            PositionClickerLog("START/STOP: Try to press this combination: SHIFT+ALT+L");

            if (_altShiftO.Register())
                PositionClickerLog("Hotkey: Alt+Shift+O registered");
            else
                PositionClickerLog("Hotkey: Alt+Shift+O failed to registered!");

            if (_altShiftK.Register())
                PositionClickerLog("Hotkey: Alt+Shift+K registered");
            else
                PositionClickerLog("Hotkey: Alt+Shift+K failed to registered!");

            if (_altShiftL.Register())
                PositionClickerLog("Hotkey: Alt+Shift+L registered");
            else
                PositionClickerLog("Hotkey: Alt+Shift+L failed to registered!");
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
            _isPosClickerStarted = false;
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
        /*
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
                clickDelay = (int)clickerDeleySpin.Value;
                clickerDeleySpin.Enabled = false;
                _countLabel.Text = strCount + clicksCount;
                clickThread = new Thread(ClickerThread);
                clickThread.Start();
                ClickerLog("Clicking Started");
            }
            else
            {
                isClickerStarted = false;
                allClicksCount += clicksCount;
                clickerDeleySpin.Enabled = true;
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
            logTxtBox.Text += text + Environment.NewLine;
            logTxtBox.SelectionStart = logTxtBox.Text.Length;
            logTxtBox.ScrollToCaret();

        }

        #endregion
        */
        #region Tab "Position Clicker"

        /// <summary>
        /// Position Clicker process
        /// </summary>
        public void PositionClickerMainProcess()
        {
            if (!_isPosClickerStarted)
            {
                _isPosClickerStarted = true;
                posClickerCtrlsPnl.Enabled = false;
                _posClickDelay = (int)posClickerSpin.Value;
                _posClickThread = new Thread(PositionClickerThread);
                _posClickThread.Start();
                PositionClickerLog("Clicking Started");
            }
            else
            {
                _isPosClickerStarted = false;
                posClickerCtrlsPnl.Enabled = true;
                PositionClickerLog("Clicking Stopped");
                posClickerCirclesLbl.Text = StrCircles + _circlesCount;
            }
        }

        public void PositionClickerThread()
        {
            while (_isPosClickerStarted)
            {
                for (int i = 0; i < posClickerGrid.RowCount && _isPosClickerStarted; i++)
                {
                    string[] resultsArray = posClickerGrid.Rows[i].Cells[0].Value.ToString().Split(',');
                    MoveCursor(new Point(Convert.ToInt32(resultsArray[0]), Convert.ToInt32(resultsArray[1])));
                    DoMouseClick();
                    Thread.Sleep(_posClickDelay);
                }
                _circlesCount++;
            }
        }

        /// <summary>
        /// Adding position info to GridView
        /// </summary>
        private void AddData()
        {
            if (!_isPosClickerStarted)
                if (Cursor.Current != null)
                {
                    Cursor = new Cursor(Cursor.Current.Handle);
                    posClickerGrid.Rows.Add(Cursor.Position.X + "," + Cursor.Position.Y);
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
            if (posClickerGrid.Rows.Count > 0)
                try
                {
                    int selectedIndex = posClickerGrid.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        posClickerGrid.Rows.RemoveAt(selectedIndex);
                        PositionClickerLog("Postition removed");
                    }
                }
                catch (InvalidOperationException)
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
            if (posClickerGrid.Rows.Count > 0)
            {
                while (posClickerGrid.Rows.Count > 0)
                {
                    posClickerGrid.Rows.RemoveAt(posClickerGrid.Rows.Count - 1);
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
            if (posClickerGrid.Rows.Count > 0)
            {
                int selectedIndex = posClickerGrid.CurrentCell.RowIndex;
                if (selectedIndex > -1)
                {
                    if (selectedIndex + jumps >= 0 && selectedIndex + jumps < posClickerGrid.Rows.Count)
                    {
                        posClickerGrid.Rows[selectedIndex].IsSelected = false;
                        posClickerGrid.Rows.Move(selectedIndex, selectedIndex + jumps);
                        posClickerGrid.Rows[selectedIndex + jumps].IsSelected = true;
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
            logTxtBox.Text += text + Environment.NewLine;
            logTxtBox.SelectionStart = logTxtBox.Text.Length;
            logTxtBox.ScrollToCaret();
        }

        #endregion
    }
}
