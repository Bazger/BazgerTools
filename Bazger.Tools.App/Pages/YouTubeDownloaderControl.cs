using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bazger.Tools.App.Properties;
using Bazger.Tools.App.State;
using Bazger.Tools.App.Utils;
using Bazger.Tools.YouTubeDownloader.Core;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using Microsoft.WindowsAPICodePack.Dialogs;
using NLog;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Pages
{
    public partial class YouTubeDownloaderControl : UserControl, IToolControl, IControlStateChanger
    {
        private MainForm _mainForm;
        public RadPageViewPage ParentPage { get; set; }
        public string Title => this.GetType().FullName;
        private Logger Log = LogManager.GetCurrentClassLogger();

        private MainLauncher _launcher;
        private ManualResetEvent _stopEvent;
        private string _currentUrl;
        private List<string> _videoUrls;
        private DownloaderConfigs _downloaderConfigs;
        private Thread _gridUpdate;
        private Thread _progressBarUpdate;
        private RadGridView _videoStageGrid;
        private RadGridView _videoStageStatsGrid;
        private bool _isStarted;

        public YouTubeDownloaderControl()
        {
            InitializeComponent();
            _isStarted = false;
            //TODO: test journal
            //TODO: fix resizing
            //TODO: dropdown list
            //TODO: remove Video Stage from form class
        }

        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;
            _videoStageGrid = _mainForm.videoStageGrid;
            _videoStageGrid.CellFormatting += radGridView_CellFormatting;
            _videoStageStatsGrid = _mainForm.videoStageStatsGrid;
            _mainForm.AddRuleToPageLoggerTarget("Bazger.Tools.YouTubeDownloader.Core.*", LogLevel.Info, Title);
            startBtn.DropDownButtonElement.ActionButton.Click += startBtn_Click;
        }

        public void LoadState(IControlState controlState)
        {
            var state = controlState as YouTubeDownloaderControlState;
            if (state == null)
            { return; }

            if (!string.IsNullOrEmpty(state.Url))
            {
                urlTxtBox.Text = state.Url;
            }
            converterThreadsSpin.Value = state.ConvertersThreadSpin;
            //TODO: state.ConvertionFormat;
            if (convertionFormatsDropDownList.Items.Contains(state.ConvertionFormat))
            {
                convertionFormatsDropDownList.Text = state.ConvertionFormat;
            }
            downloaderThreadsSpin.Value = state.DownloadersThreadSpin;
            //downloadsFolderDropDown.Text = state.DownloadsFolderPath;
            overwriteFilesChkBox.Checked = state.IsOverwriteChecked;
            readFromJournalChkBox.Checked = state.IsReadFromJournalChecked;
            writeToJournalChkBox.Checked = state.IsWriteToJournalCheked;
            //journalFileDropDown.Text = state.JournalFilePath;
            //TODO: state.VideoFormat;
        }

        public IControlState SaveState()
        {
            return new YouTubeDownloaderControlState
            {
                Url = _currentUrl,
                ConvertersThreadSpin = (int)converterThreadsSpin.Value,
                DownloadersThreadSpin = (int)downloaderThreadsSpin.Value,
                DownloadsFolderPath = downloadsFolderDropDown.Text,
                IsOverwriteChecked = overwriteFilesChkBox.Checked,
                IsReadFromJournalChecked = readFromJournalChkBox.Checked,
                IsWriteToJournalCheked = writeToJournalChkBox.Checked,
                JournalFilePath = journalFileDropDown.Text,
                ConvertionFormat = convertionFormatsDropDownList.Text
            };
        }

        public void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _gridUpdate?.Abort();
            _progressBarUpdate?.Abort();
            _launcher?.ForceStop();
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
            if (string.IsNullOrEmpty(urlTxtBox.Text))
            {
                urlTxtBox.Text = Resources.youtubeDonwloaderUrlInfo;
                startBtn.Enabled = false;
            }
            else
            {
                startBtn.Enabled = true;
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!_isStarted)
            {
                _currentUrl = urlTxtBox.Text;
                if (!ValidateUrl(_currentUrl))
                {
                    Log.Warn($"Entered url is not correct, try new one | url={_currentUrl}");
                    return;
                }
                ToWorkState();
                _stopEvent = new ManualResetEvent(false);
                new Thread(StandardDownloading) { Name = "Starter" }.Start();
            }
            else
            {
                new Thread(StopDownloading) { Name = "Stopper" }.Start();
            }
        }

        private void previewMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ToIdleState()
        {
            _isStarted = false;
            ControlInvoker(startBtn, control => { control.Text = Resources.startBtn_Start; });
            ControlInvoker(urlTxtBox, control => { control.Enabled = true; });
            ControlInvoker(converterPnl, control => { control.Enabled = true; });
            ControlInvoker(pathsPnl, control => { control.Enabled = true; });
            ControlInvoker(threadPnl, control => { control.Enabled = true; });
            ControlInvoker(startBtn, control => { control.Enabled = true; });
        }

        private void ToWorkState()
        {
            _isStarted = true;
            ControlInvoker(startBtn, control => { control.Text = Resources.startBtn_Stop; });
            ControlInvoker(converterPnl, control => { control.Enabled = false; });
            ControlInvoker(urlTxtBox, control => { control.Enabled = false; });
            ControlInvoker(pathsPnl, control => { control.Enabled = false; });
            ControlInvoker(threadPnl, control => { control.Enabled = false; });
            ControlInvoker(_videoStageGrid, control => { control.Rows.Clear(); });
            ControlInvoker(downloadProgressBar, bar =>
            {
                bar.Value1 = 0;
                bar.Text = "0%";
                bar.Visible = false;
            });
            ControlInvoker(waitingBar, bar =>
            {
                bar.Visible = true;
                bar.Text = "Trying to get all video urls";
            });
            ControlInvoker(startBtn, control => { control.Items.Clear(); });
        }

        private void StandardDownloading()
        {
            LoadAllVideoUrls();

            if (_stopEvent.WaitOne(0))
            {
                ToIdleState();
                return;
            }

            _launcher = new MainLauncher(_videoUrls, _downloaderConfigs);
            _launcher.Start();

            _gridUpdate = new Thread(GridUpdate) { Name = "GridUpdate" };
            _gridUpdate.Start();

            while (!_launcher.WaitForStop(1000))
            {
                //Waiting for finishing or stop event call
            }
            ToIdleState();
        }

        private void LoadAllVideoUrls()
        {
            _progressBarUpdate = new Thread(ProgressBarUpdate) { Name = "ProgressBarUpdate" };
            _progressBarUpdate.Start();

            _downloaderConfigs = new DownloaderConfigs()
            {
                YouTubeApiKey = Resources.youtubeApiKey,
                SaveDir = downloadsFolderDropDown.Text,
                YouTubeVideoFormatCode = 18,
                ParallelDownloadsCount = (int)downloaderThreadsSpin.Value,
                ConvertersCount = (int)converterThreadsSpin.Value,
                ConverterEnabled = convertionEnabledChkBox.IsChecked,
                ConvertionFormat = convertionFormatsDropDownList.Text,
                JournalFilePath = journalFileDropDown.Text,
                ReadFromJournal = readFromJournalChkBox.IsChecked,
                WriteToJournal = writeToJournalChkBox.IsChecked,
                OverwriteEnabled = overwriteFilesChkBox.IsChecked
            };

            var getVideoUrlsTask = new Task(() => { _videoUrls = GetVideoUrls(); });
            getVideoUrlsTask.Start();

            while (!getVideoUrlsTask.Wait(300))
            {
                if (_stopEvent.WaitOne(0))
                {
                    return;
                }
            }

            if (_videoUrls == null)
            {
                _stopEvent.Set();
                return;
            }

            ControlInvoker(_videoStageGrid, stageGrid =>
            {
                var i = 1;
                stageGrid.BeginUpdate();
                _videoUrls.ForEach(url =>
                {
                    stageGrid.Rows.Add(i++, url);
                });
                stageGrid.TableElement.RowScroller.ScrollToFirstRow();
                stageGrid.EndUpdate();
            });
            ControlInvoker(_videoStageStatsGrid, statsGrid =>
            {
                statsGrid.Rows.Add(GetVideoStageStatsRow().GetAllParams());
            });
        }

        private void StopDownloading()
        {
            ControlInvoker(startBtn, control =>
            {
                control.Enabled = false;
            });
            _stopEvent.Set();

            _launcher?.Stop();
        }


        private VideoStageStats GetVideoStageStatsRow()
        {
            var stats = new VideoStageStats { All = _videoUrls.Count };
            foreach (var video in _launcher.VideosProgress)
            {
                switch (video.Value.Stage)
                {
                    case VideoProgressStage.Error:
                        stats.Errors++;
                        break;
                    case VideoProgressStage.Converting:
                        stats.Converting++;
                        break;
                    case VideoProgressStage.WaitingToConvertion:
                        stats.Waiting++;
                        break;
                    case VideoProgressStage.Completed:
                        stats.Completed++;
                        break;
                    case VideoProgressStage.VideoUrlProblem:
                        stats.UrlProblem++;
                        break;
                    case VideoProgressStage.Moving:
                        stats.Moving++;
                        break;
                    case VideoProgressStage.Downloading:
                        stats.Downloading++;
                        break;
                    case VideoProgressStage.Exist:
                        break;
                }
            }

            return stats;
        }

        private class VideoStageStats
        {
            public int All { private get; set; }
            public int Downloading { get; set; }
            public int Converting { get; set; }
            public int Completed { get; set; }
            public int Errors { get; set; }
            public int UrlProblem { get; set; }
            public int Waiting { get; set; }
            public int Moving { get; set; }

            public object[] GetAllParams()
            {
                return new object[]
                {
                    Downloading, Converting, $"{Completed}/{All}", Errors,
                    UrlProblem, Waiting, Moving
                };
            }
        }

        private List<string> GetVideoUrls()
        {
            try
            {
                Log.Info("Trying to get all video urls");
                var videoUrls = YouTubeHelper.GetVideosUrls(_currentUrl, _downloaderConfigs.YouTubeApiKey).ToList();
                if (videoUrls.Any())
                {
                    return videoUrls;
                }
                Log.Error("Your playlist or video url not contains video or null");
            }
            catch (Exception ex)
            {
                Log.Error("Check your dowload url or Api key, there are may be incorrect \n" + ex);
            }
            return null;
        }

        private void GridUpdate()
        {
            while (!_launcher.WaitForStop(1000))
            {
                ControlInvoker(_videoStageGrid, stageGrid =>
                {
                    foreach (var row in stageGrid.Rows)
                    {
                        var url = row.Cells["url"].Value.ToString();
                        if (!_launcher.VideosProgress.ContainsKey(url))
                        {
                            continue;
                        }
                        row.Cells["progress"].Value = _launcher.VideosProgress[url].Progress;
                        row.Cells["stage"].Value = _launcher.VideosProgress[url].Stage;
                        row.Cells["title"].Value = _launcher.VideosProgress[url].Title;
                    }
                });
                ControlInvoker(_videoStageStatsGrid, stageGrid =>
                {
                    var row = stageGrid.Rows.First();
                    if (row == null)
                    {
                        return;
                    }
                    var stats = GetVideoStageStatsRow().GetAllParams();
                    for (var i = 0; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Value = stats[i];
                    }
                });
            }
        }

        private static Color? StageColorFactory(VideoProgressStage stage)
        {
            switch (stage)
            {
                case VideoProgressStage.Completed:
                    return Color.LimeGreen;
                case VideoProgressStage.Error:
                    return Color.Red;
                case VideoProgressStage.Converting:
                    return Color.Yellow;
                case VideoProgressStage.Moving:
                case VideoProgressStage.WaitingToConvertion:
                    return Color.Gray;
                case VideoProgressStage.VideoUrlProblem:
                    return Color.HotPink;
                case VideoProgressStage.Downloading:
                    return Color.LightGreen;
                default:
                    return null;
            }
        }

        private void radGridView_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            var dataRow = e.CellElement.RowInfo as GridViewDataRowInfo;

            if (dataRow == null)
            {
                return;
            }

            var columnValue = dataRow.Cells["stage"].Value;
            if (e.Column.Name == "stage" && columnValue != null)
            {
                e.CellElement.DrawFill = true;
                var color = StageColorFactory(_launcher.VideosProgress[e.Row.Cells["url"].Value.ToString()].Stage);
                if (color != null)
                {
                    e.CellElement.BackColor = color.Value;
                }
                e.CellElement.ForeColor = Color.Black;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
            }

            if (e.Column.Name == "quality")
            {
                var comboBoxColumn = e.Column as GridViewComboBoxColumn;
                if (comboBoxColumn != null)
                {
                    comboBoxColumn.DataSource = new[] {"VASYA"};
                }
            }
        }

        private void ProgressBarUpdate()
        {
            const int waitingTimeout = 500;
            IsWaitingBar(true);
            while (!_stopEvent.WaitOne(waitingTimeout) && (_launcher == null || (_launcher != null && !_launcher.IsAlive)))
            {
                ControlInvoker(waitingBar, bar => { bar.Text = "Trying to get all video urls"; });
            }
            IsWaitingBar(false);
            if (_launcher == null || _stopEvent.WaitOne(0))
            {
                return;
            }
            while (!_launcher.WaitForStop(0) && !_stopEvent.WaitOne(waitingTimeout))
            {
                ControlInvoker(downloadProgressBar, bar =>
                {
                    var sum = _launcher.VideosProgress.Values.Sum(v =>
                    {
                        if (v.Stage == VideoProgressStage.Completed)
                        {
                            return v.Progress;
                        }
                        //If video not finished to downloading it will be differed by 3 percent
                        return 0.97 * v.Progress;
                    });
                    //Staging 1 percent to finish downloading
                    var progress = sum / (_videoUrls.Count * 100) * 99;
                    bar.Value1 = (int)progress;
                    bar.Text = (int)progress + "%";
                });
            }

            if (!_launcher.WaitForStop(0))
            {
                ControlInvoker(waitingBar, bar => bar.Text = "Stopping");
                IsWaitingBar(true);
                while (!_launcher.WaitForStop(waitingTimeout))
                {
                }
            }

            if (_stopEvent.WaitOne(0))
            {
                IsWaitingBar(true);
                ControlInvoker(waitingBar, bar =>
                {
                    bar.WaitingIndicatorSize = new Size(0, 100);
                    bar.Text = "Stopped";
                });
            }
            else
            {
                ControlInvoker(downloadProgressBar, bar =>
                {
                    bar.Value1 = 100;
                    bar.Text = 100 + "%";
                });
            }
        }

        private void IsWaitingBar(bool flag)
        {
            if (flag)
            {
                ControlInvoker(downloadProgressBar, bar => bar.Visible = false);
                ControlInvoker(waitingBar, bar =>
                {
                    bar.Visible = true;
                    bar.StartWaiting();
                    bar.WaitingIndicatorSize = new Size(100, 100);
                });
            }
            else
            {
                ControlInvoker(downloadProgressBar, bar => bar.Visible = true);
                ControlInvoker(waitingBar, bar =>
                {
                    bar.Visible = false;
                    bar.StopWaiting();
                    bar.WaitingIndicatorSize = new Size(0, 100);
                });
            }
        }

        private static void ControlInvoker<T>(T control, Action<T> action) where T : ScrollableControl, new()
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => { action(control); }));
            }
            else
            {
                action(control);
            }
        }

        private static bool ValidateUrl(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private void urlTxtBox_TextChanging(object sender, Telerik.WinControls.TextChangingEventArgs e)
        {
            startBtn.Enabled = !string.IsNullOrEmpty(e.NewValue);
        }

        private void goToFolderBtn_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(downloadsFolderDropDown.Text))
            {
                Process.Start(downloadsFolderDropDown.Text);
            }
        }

        private void browseJournalBtn_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog() { Filter = "Json Files (.json)|*.json|All Files (*.*)|*.*" })
            {
                DialogResult result = ofd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.SafeFileName))
                {
                    journalFileDropDown.Text = ofd.FileName;
                }
            }
        }

        private void browseDownloadsBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    downloadsFolderDropDown.Text = dialog.FileName;
                }
            }
        }

        private void downloadsFolderDropDown_TextChanged(object sender, EventArgs e)
        {
            var text = downloadsFolderDropDown.Text;
            if (string.IsNullOrEmpty(text) || !text.Contains(":\\"))
            {
                return;
            }
            try
            {
                var selectionStart = downloadsFolderDropDown.SelectionStart;
                var fullPath = Path.GetFullPath(text);
                var directory = Path.GetDirectoryName(fullPath);
                Debug.WriteLine("FullPath - " + fullPath);
                Debug.WriteLine("Directory - " + directory);
                if (string.IsNullOrEmpty(directory) && Directory.Exists(text))
                {
                    directory = text;
                }
                else if (!Directory.Exists(directory) || !Path.IsPathRooted(text))
                {
                    return;
                }
                //Check if entered text has been changed and not when only selected index.
                if (downloadsFolderDropDown.SelectedItem == null)
                {
                    //downloadsFolderDropDown.AutoCompleteDataSource = Directory.GetDirectories(directory).ToList();
                    //downloadsFolderDropDown.AutoCompleteDisplayMember = text;                    
                    var allDirectories =
                        Directory.GetDirectories(directory)
                            .Where(dir => dir.ToLower().Contains(text.ToLower()))
                            .ToList();
                    allDirectories.Insert(0, text);
                    downloadsFolderDropDown.DataSource = allDirectories;
                    downloadsFolderDropDown.ShowDropDown();
                    downloadsFolderDropDown.SelectionStart = selectionStart;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void downloadsFolderDropDown_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            downloadsFolderDropDown.SelectionStart = downloadsFolderDropDown.Text.Length;
        }
    }
}