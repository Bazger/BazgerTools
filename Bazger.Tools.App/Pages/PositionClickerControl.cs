﻿using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Bazger.Tools.App.State;
using Bazger.Tools.WinApi;
using NLog;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Pages
{
    public partial class PositionClickerControl : UserControl, IToolControl, IControlStateChanger
    {
        private MainForm _mainForm;
        public string LoggerName => this.GetType().FullName;
        public RadPageViewPage ParentPage { get; set; }
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

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
        }

        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;
            mainForm.AltShiftKCombinationPressed += AddData;
            mainForm.AltShiftLCombinationPressed += PositionClickerMainProcess;
            _log.Info("ADD: Try to press this combination: SHIFT+ALT+K");
            _log.Info("START/STOP: Try to press this combination: SHIFT+ALT+L");
        }

        public void LoadState(IControlState controlState)
        {
            if (!(controlState is PositionClickerControlState state))
            { return; }

            delaySpin.Value = state.DelaySpin;
            foreach (var row in state.PositionsGrid)
            {
                var regex = new Regex("^[0-9]+,[0-9]+$");
                if (!regex.IsMatch(row))
                {
                    continue;
                }
                positionsGrid.Rows.Add(row);
            }
        }

        public IControlState SaveState()
        {
            return new PositionClickerControlState
            {
                DelaySpin = (int)delaySpin.Value,
                PositionsGrid = positionsGrid.Rows.Select(r => r.Cells["position"].Value.ToString()).ToList()
            };
        }

        public void OnFormClosing(object sender, FormClosingEventArgs e)
        {
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
                _log.Info("Clicking Started");
                _mainForm.IsPageSwitichingEnabled(false);
            }
            else
            {
                _isPosClickerStarted = false;
                ctrlsPnl.Enabled = true;
                _log.Info("Clicking Stopped");
                circlesLbl.Text = StrCircles + _circlesCount;
                _mainForm.IsPageSwitichingEnabled(true);
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
                    MouseHelper.DoMouseLeftClick(Cursor.Position.X, Cursor.Position.Y);
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
                    _log.Info("Position (" + Cursor.Position.X + ", " + Cursor.Position.Y + ") was added");
                }
                else
                {
                    _log.Error("Can't add cursor position!");
                }
            else
            {
                _log.Warn("Clicking thread is working! Stop it for adding a new position");
            }

        }

        /// <summary>
        /// Removing data on current position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (positionsGrid.Rows.Count > 0)
                try
                {
                    int selectedIndex = positionsGrid.CurrentCell.RowIndex;
                    if (selectedIndex > -1)
                    {
                        positionsGrid.Rows.RemoveAt(selectedIndex);
                        _log.Info("Postition removed");
                    }
                }
                catch (InvalidOperationException)
                {
                    _log.Warn("Unable to remove selected row at this time");
                }
            else
                _log.Info("Select the row");

        }

        /// <summary>
        /// Removing all data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAllButton_Click(object sender, EventArgs e)
        {
            if (positionsGrid.Rows.Count > 0)
            {
                while (positionsGrid.Rows.Count > 0)
                {
                    positionsGrid.Rows.RemoveAt(positionsGrid.Rows.Count - 1);
                }
                _log.Info("All data was removed");
            }
        }

        /// <summary>
        /// Button "Move Up" event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            MoveUpDown(-1);
        }

        /// <summary>
        /// Button "Move Down" event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void MoveDownButton_Click(object sender, EventArgs e)
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
                int selectedIndex = positionsGrid.CurrentCell?.RowIndex ?? -1;
                if (selectedIndex > -1)
                {
                    if (selectedIndex + jumps >= 0 && selectedIndex + jumps < positionsGrid.Rows.Count)
                    {
                        positionsGrid.Rows[selectedIndex].IsSelected = false;
                        positionsGrid.Rows.Move(selectedIndex, selectedIndex + jumps);
                        positionsGrid.Rows[selectedIndex + jumps].IsSelected = true;
                        positionsGrid.Rows[selectedIndex + jumps].IsCurrent = true;
                        positionsGrid.Rows[selectedIndex + jumps].EnsureVisible();
                        _log.Info("Postition moved UP/DOWN");
                    }

                }
            }
        }
    }
}
