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
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Pages
{
    public partial class YouTubeDownloaderControl : UserControl, IToolControl
    {
        private MainForm _mainForm;
        public RadPageViewPage ParentPage { get; set; }
        public List<string> Log { get; }

        public YouTubeDownloaderControl()
        {
            InitializeComponent();

            //TODO: LOG SUPPORT
            Log = new List<string>();
        }

        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        /// <summary>
        /// Clicker Log
        /// </summary>
        /// <param name="text">String that will be added</param>
        private static void DownloaderLog(string text)
        {
            //logTxtBox.Text += text + Environment.NewLine;
            //logTxtBox.SelectionStart = logTxtBox.Text.Length;
            //logTxtBox.ScrollToCaret();
        }
    }
}
