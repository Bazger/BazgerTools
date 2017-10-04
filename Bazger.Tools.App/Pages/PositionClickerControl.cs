using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Bazger.Tools.Clicker.Core;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Pages
{
    public partial class PositionClickerControl : UserControl, IToolControl
    {
        private MainForm _mainForm;
        public RadPageViewPage ParentPage { get; set; }
        public List<string> Log { get; }

        private const string StrCircles = "Circles Count: ";
        private Thread _posClickThread;


        private int _circlesCount;
        private int _posClickDelay;
        private bool _isPosClickerStarted;


        public PositionClickerControl()
        {
            InitializeComponent();

            _circlesCount = 0;
            _posClickDelay = 10;
            circlesLbl.Text = StrCircles + _circlesCount;
            _isPosClickerStarted = false;
            delaySpin.Value = _posClickDelay;

            //TODO: LOG SUPPORT
            Log = new List<string>();
            PositionClickerLog("ADD: Try to press this combination: SHIFT+ALT+K");
            PositionClickerLog("START/STOP: Try to press this combination: SHIFT+ALT+L");
        }

        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;
            mainForm.AltShiftKCombinationPressed += AddData;
            mainForm.AltShiftLCombinationPressed += PositionClickerMainProcess;
        }

        /// <summary>
        /// Set the Current cursor, move the cursor's Position, and set its clipping rectangle to the form. 
        /// </summary>
        /// <param name="pos">next movment position</param>
        private static void MoveCursor(Point pos)
        {
            Cursor.Position = pos;
        }

        /// <summary>
        /// Position Clicker process
        /// </summary>
        private void PositionClickerMainProcess(object sender, PageEventArgs e)
        {
            if (ParentPage.TabIndex != e.SelectedPage.TabIndex)
            {
                return;
            }
            if (!_isPosClickerStarted)
            {
                _isPosClickerStarted = true;
                ctrlsPnl.Enabled = false;
                _posClickDelay = (int)delaySpin.Value;
                _posClickThread = new Thread(PositionClickerThread);
                _posClickThread.Start();
                PositionClickerLog("Clicking Started");
                _mainForm.ChangeEnabledStateOfNotSelectedTabs(false);
            }
            else
            {
                _isPosClickerStarted = false;
                ctrlsPnl.Enabled = true;
                PositionClickerLog("Clicking Stopped");
                circlesLbl.Text = StrCircles + _circlesCount;
                _mainForm.ChangeEnabledStateOfNotSelectedTabs(true);
            }
        }


        private void PositionClickerThread()
        {
            while (_isPosClickerStarted)
            {
                for (int i = 0; i < positionsGrid.RowCount && _isPosClickerStarted; i++)
                {
                    string[] resultsArray = positionsGrid.Rows[i].Cells[0].Value.ToString().Split(',');
                    MoveCursor(new Point(Convert.ToInt32(resultsArray[0]), Convert.ToInt32(resultsArray[1])));
                    ProgramClick.DoMouseClick(Cursor.Position.X, Cursor.Position.Y);
                    Thread.Sleep(_posClickDelay);
                }
                _circlesCount++;
            }
        }

        /// <summary>
        /// Adding position info to GridView
        /// </summary>
        private void AddData(object sender, PageEventArgs e)
        {
            if (ParentPage.TabIndex != e.SelectedPage.TabIndex)
            {
                return;
            }
            if (!_isPosClickerStarted)
                if (Cursor.Current != null)
                {
                    Cursor = new Cursor(Cursor.Current.Handle);
                    positionsGrid.Rows.Add(Cursor.Position.X + "," + Cursor.Position.Y);
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
            if (positionsGrid.Rows.Count > 0)
                try
                {
                    int selectedIndex = positionsGrid.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        positionsGrid.Rows.RemoveAt(selectedIndex);
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
            if (positionsGrid.Rows.Count > 0)
            {
                while (positionsGrid.Rows.Count > 0)
                {
                    positionsGrid.Rows.RemoveAt(positionsGrid.Rows.Count - 1);
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
            if (positionsGrid.Rows.Count > 0)
            {
                int selectedIndex = positionsGrid.CurrentCell.RowIndex;
                if (selectedIndex > -1)
                {
                    if (selectedIndex + jumps >= 0 && selectedIndex + jumps < positionsGrid.Rows.Count)
                    {
                        positionsGrid.Rows[selectedIndex].IsSelected = false;
                        positionsGrid.Rows.Move(selectedIndex, selectedIndex + jumps);
                        positionsGrid.Rows[selectedIndex + jumps].IsSelected = true;
                        PositionClickerLog("Postition moved UP/DOWN");
                    }

                }
            }
        }

        /// <summary>
        /// Position Clicker Log
        /// </summary>
        /// <param name="text">String that will be added</param>
        private static void PositionClickerLog(string text)
        {
//            logTxtBox.Text += text + Environment.NewLine;
//            logTxtBox.SelectionStart = logTxtBox.Text.Length;
//            logTxtBox.ScrollToCaret();
        }
    }
}
