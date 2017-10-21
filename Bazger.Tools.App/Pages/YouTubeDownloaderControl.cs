using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bazger.Tools.App.Properties;
using NLog;
using Telerik.WinControls.UI;
using static System.String;

namespace Bazger.Tools.App.Pages
{
    public partial class YouTubeDownloaderControl : UserControl, IToolControl
    {
        private MainForm _mainForm;
        public RadPageViewPage ParentPage { get; set; }
        private Logger Log = LogManager.GetCurrentClassLogger();

        private bool _isStarted;

        public YouTubeDownloaderControl()
        {
            InitializeComponent();
            _isStarted = false;
        }

        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        /// <summary>
        /// Clicker LogList
        /// </summary>
        /// <param name="text">String that will be added</param>
        private static void DownloaderLog(string text)
        {
            //logTxtBox.Text += text + Environment.NewLine;
            //logTxtBox.SelectionStart = logTxtBox.Text.Length;
            //logTxtBox.ScrollToCaret();
        }

        private void urlTxtBox_Focus(object sender, EventArgs e)
        {
            if (urlTxtBox.Text == Resources.youtubeDonwloaderUrlInfo)
            {
                urlTxtBox.Text = null;
            }
        }

        private void urlTxtBox_Leave(object sender, EventArgs e)
        {
            if (IsNullOrEmpty(urlTxtBox.Text))
            {
                urlTxtBox.Text = Resources.youtubeDonwloaderUrlInfo;
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!_isStarted)
            {
                _isStarted = true;
                startBtn.Text = Resources.startBtn_Stop;
                converterPnl.Enabled = false;
                pathsPnl.Enabled = false;
                threadPnl.Enabled = false;
            }
            else
            {
                _isStarted = false;
                startBtn.Text = Resources.startBtn_Start;
                converterPnl.Enabled = true;
                pathsPnl.Enabled = true;
                threadPnl.Enabled = true;
            }
        }
    }
}
