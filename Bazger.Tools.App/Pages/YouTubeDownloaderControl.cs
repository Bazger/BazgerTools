using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Bazger.Tools.App.Properties;
using Bazger.Tools.YouTubeDownloader.Core;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;
using Telerik.WinControls;
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
        private Launcher _launcher;
        private ManualResetEvent _stopEvent;
        private string _currentUrl;
        private List<string> _videoUrls;
        private DownloaderConfigs _downloaderConfigs;
        private Thread _gridUpdate;
        private Thread _progressBarUpdate;
        private RadGridView _videoStageGrid;

        public YouTubeDownloaderControl()
        {
            InitializeComponent();
            _isStarted = false;
            //TODO: show statistics
            //TODO: show video title
            //TODO: configs
            //TODO: fix resizing
            //TODO: add all logs
            //TODO: more logs
            //TODO: remove Video Stage from form class
        }

        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;
            _videoStageGrid = _mainForm.videosStageGrid;
            _videoStageGrid.CellFormatting += radGridView_RowFormatting;
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
            if (IsNullOrEmpty(urlTxtBox.Text))
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
                new Thread(StartDownloading) { Name = "Starter" }.Start();
            }
            else
            {
                new Thread(StopDownloading) { Name = "Stopper" }.Start();
            }
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
        }

        private void StartDownloading()
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
            _downloaderConfigs = DownloaderConfigs.GetDefaultConfigs();
            _videoUrls = GetVideoUrls();
            if (_stopEvent.WaitOne(0) || _videoUrls == null)
            {
                ToIdleState();
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
            _launcher = new Launcher(_videoUrls, DownloaderConfigs.GetDefaultConfigs());
            _launcher.Start();

            _gridUpdate = new Thread(GridUpdate) { Name = "GridUpdate" };
            _gridUpdate.Start();


            while (!_launcher.WaitForStop(1000))
            {
                //Waiting for finishing or stop event call
            }
            ToIdleState();
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

        private List<string> GetVideoUrls()
        {
            try
            {
                Log.Info("Trying to get all video urls");
                var videoUrls = YouTubeHelper.GetVideosUrls(
                    _currentUrl, _downloaderConfigs.YouTubeApiKey
                    ).ToList();
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
                    }
                });
            }
        }

        protected static Color StageColorFactory(VideoProgressStage stage)
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
                default:
                    return Color.LightGreen;
            }
        }

        private void radGridView_RowFormatting(object sender, CellFormattingEventArgs e)
        {
            GridViewDataRowInfo dataRow = e.CellElement.RowInfo as GridViewDataRowInfo;

            if (dataRow == null)
            {
                return;
            }

            var columnValue = dataRow.Cells["stage"].Value;
            if (e.Column.Name == "stage" && columnValue != null)
            {
                e.CellElement.DrawFill = true;
                e.CellElement.BackColor = StageColorFactory(_launcher.VideosProgress[e.Row.Cells["url"].Value.ToString()].Stage);
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
            }

        }

        private void ProgressBarUpdate()
        {
            const int waitingTimeout = 500;
            var stageEvent = new ManualResetEvent(false);
            IsWaitingBar(true);
            while (!stageEvent.WaitOne(waitingTimeout) && (_launcher == null || (_launcher != null && !_launcher.IsAlive)))
            {
                ControlInvoker(waitingBar, bar =>
                {
                    bar.Text = "Trying to get all video urls";
                });
            }
            IsWaitingBar(false);
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
                        return 0.97 * v.Progress;
                    });
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

        private static void ControlInvoker<T>(T control, Action<T> action)
            where T : ScrollableControl, new()
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

        private bool ValidateUrl(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(urlTxtBox.Text, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private void urlTxtBox_TextChanging(object sender, Telerik.WinControls.TextChangingEventArgs e)
        {
            startBtn.Enabled = !IsNullOrEmpty(e.NewValue);
        }
    }
}
