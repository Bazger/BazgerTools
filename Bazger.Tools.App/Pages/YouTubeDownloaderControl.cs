using System;
using System.Collections.Concurrent;
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
using YoutubeExtractor;
using VideoType = Bazger.Tools.YouTubeDownloader.Core.Model.VideoType;

namespace Bazger.Tools.App.Pages
{
    public partial class YouTubeDownloaderControl : UserControl, IToolControl, IControlStateChanger
    {
        private MainForm _mainForm;
        public RadPageViewPage ParentPage { get; set; }
        public string Title => this.GetType().FullName;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private IEnumerable<VideoType> AvailableVideoTypes;

        private MainLauncher _launcher;
        private ManualResetEvent _stopEvent;
        private string _currentUrl;
        private List<string> _videoUrls;

        private Thread _gridUpdate;
        private Thread _progressBarUpdate;
        private RadGridView _videoStageGrid;
        private RadGridView _videoStageStatsGrid;
        private bool _isStarted;
        private bool _isPreview;
        private PreviewLauncher _previewLauncher;
        private IDictionary<string, VideoProgressMetadata> _videosProgress;

        public YouTubeDownloaderControl()
        {
            InitializeComponent();
            _isStarted = false;
            _isPreview = false;
            //TODO: Test journal
            //TODO: Fix resizing
            //TODO: Dropdown list
            //TODO: Remove Video Stage from form class
            //TODO: Disable editing for all columns excepts for video_types
            //TODO: After stop pressing the state will be returned to previous stage
            //TODO: Rerun from preview after finishing                      
            //TODO: Add option to download again with changing preview
            //TODO: Bug fix - can't do preview more than one time
            //TODO: Set stat button disable at the startup when url is empty
            //TODO: Show error when now preview
        }


        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;
            _videoStageGrid = _mainForm.videoStageGrid;
            _videoStageGrid.CellFormatting += videoStageGrid_CellFormatting;
            _videoStageGrid.CellValueChanged += videoStageGrid_CellValueChanged;
            _videoStageGrid.CellBeginEdit += videoStageGrid_CellClick;
            _videoStageStatsGrid = _mainForm.videoStageStatsGrid;
            _mainForm.AddRuleToPageLoggerTarget("Bazger.Tools.YouTubeDownloader.Core.*", LogLevel.Info, Title);
            startBtn.DropDownButtonElement.ActionButton.Click += startBtn_Click;
            videoTypesDropDown.DataSource = VideoType.AvailabledVideoTypes;
            AvailableVideoTypes = VideoType.AvailabledVideoTypes;
        }

        public void LoadState(IControlState controlState)
        {
            if (!(controlState is YouTubeDownloaderControlState state))
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
            convertionEnabledChkBox.Checked = state.IsConversionChecked;
            //journalFileDropDown.Text = state.JournalFilePath;
            //TODO: state.VideoFormat;
        }

        public IControlState SaveState()
        {
            return new YouTubeDownloaderControlState
            {
                Url = _currentUrl, //TODO: Fix url removing
                DownloadersThreadSpin = (int)downloaderThreadsSpin.Value,
                ConvertersThreadSpin = (int)converterThreadsSpin.Value,
                IsConversionChecked = convertionEnabledChkBox.Checked,
                ConvertionFormat = convertionFormatsDropDownList.Text,
                DownloadsFolderPath = downloadsFolderDropDown.Text,
                IsOverwriteChecked = overwriteFilesChkBox.Checked,
                IsReadFromJournalChecked = readFromJournalChkBox.Checked,
                IsWriteToJournalCheked = writeToJournalChkBox.Checked,
                JournalFilePath = journalFileDropDown.Text,
            };
        }

        public void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _gridUpdate?.Abort();
            _progressBarUpdate?.Abort();
            _launcher?.Abort();
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
                _stopEvent = new ManualResetEvent(false);
                ToWorkState();
                new Thread(VideoDownloading) { Name = "Starter" }.Start();
            }
            else
            {
                new Thread(StopDownloading) { Name = "Stopper" }.Start();
            }
        }

        private void previewMenuItem_Click(object sender, EventArgs e)
        {
            _currentUrl = urlTxtBox.Text;
            if (!ValidateUrl(_currentUrl))
            {
                Log.Warn($"Entered url is not correct, try new one | url={_currentUrl}");
                return;
            }
            ToWorkState();
            _stopEvent = new ManualResetEvent(false);
            new Thread(GetVideosPreview) { Name = "Previewer" }.Start();
        }

        private void ToIdleState()
        {
            _isStarted = false;
            FormHelper.ControlInvoker(urlTxtBox, control => { control.Enabled = true; });
            FormHelper.ControlInvoker(converterPnl, control => { control.Enabled = true; });
            FormHelper.ControlInvoker(pathsPnl, control => { control.Enabled = true; });
            FormHelper.ControlInvoker(threadPnl, control => { control.Enabled = true; });
            if (!_isPreview)
            {
                FormHelper.ControlInvoker(startBtn, control => { control.Text = Resources.startBtn_Start; });
                FormHelper.ControlInvoker(startBtn, control => { control.Enabled = true; });
                FormHelper.ControlInvoker(startBtn, control => { control.Items.Add(previewMenuItem); });
            }
            else
            {
                FormHelper.ControlInvoker(startBtn, control => { control.Text = Resources.startBtn_Download; });
                FormHelper.ControlInvoker(startBtn, control => { control.Enabled = true; });
                FormHelper.ControlInvoker(startBtn, control => { control.Items.AddRange(startMenuItem, previewMenuItem); });
            }
        }


        private void ToWorkState()
        {
            _isStarted = true;
            FormHelper.ControlInvoker(startBtn, control => { control.Text = Resources.startBtn_Stop; });
            FormHelper.ControlInvoker(converterPnl, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(urlTxtBox, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(pathsPnl, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(threadPnl, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(_videoStageGrid, control => { control.Rows.Clear(); });
            FormHelper.ControlInvoker(downloadProgressBar, bar =>
            {
                bar.Value1 = 0;
                bar.Text = "0%";
                bar.Visible = false;
            });
            FormHelper.ControlInvoker(waitingBar, bar =>
            {
                bar.Visible = true;
                bar.Text = "Trying to get all video urls";
            });
            FormHelper.ControlInvoker(startBtn, control => { control.Items.Clear(); });
        }


        private void GetVideosPreview()
        {
            LoadAllVideoUrls();

            if (_stopEvent.WaitOne(0))
            {
                ToIdleState();
                return;
            }

            _previewLauncher = new PreviewLauncher(_videoUrls);
            _previewLauncher.Start();

            _videosProgress = _previewLauncher.VideosProgress;

            InitializeGrids();

            var previewGridUpdate = new Thread(PreviewGridUpdate) { Name = "PreviewGridUpdate" };
            previewGridUpdate.Start();

            while (!_previewLauncher.Wait(1000))
            {
                //Waiting for finishing or stop event call
            }
            if (!_stopEvent.WaitOne(0))
            {
                _isPreview = true;
            }
            ToIdleState();
            _stopEvent.Set();
        }

        private void VideoDownloading()
        {
            if (!_isPreview)
            {
                LoadAllVideoUrls();
                if (_stopEvent.WaitOne(0))
                {
                    return;
                }
                _launcher = new MainLauncher(_videoUrls, GetDownloaderConfigs());
                _launcher.Start();
                _videosProgress = _launcher.VideosProgress;
            }
            else
            {
                //TODO: send videoUrls for better sequence
                _launcher = new MainLauncher(_videosProgress, GetDownloaderConfigs());
                _launcher.Start();
                _videosProgress = _launcher.VideosProgress;
            }

            InitializeGrids();

            _gridUpdate = new Thread(GridUpdate) { Name = "GridUpdate" };
            _gridUpdate.Start();

            while (!_launcher.Wait(1000))
            {
                //Waiting for finishing or stop event call
            }
            _stopEvent.Set();
            _isPreview = false;
            ToIdleState();
        }


        private void LoadAllVideoUrls()
        {
            _progressBarUpdate = new Thread(ProgressBarUpdate) { Name = "ProgressBarUpdate" };
            _progressBarUpdate.Start();

            var videoUrlsReceiverTask = new Task(() =>
            {
                Thread.CurrentThread.Name = "UrlsReceiver";
                var stopEvent = _stopEvent;
                var videoUrls = GetVideoUrls();
                if (!stopEvent.WaitOne(0))
                {
                    _videoUrls = videoUrls;
                }
            });
            videoUrlsReceiverTask.Start();

            while (!videoUrlsReceiverTask.Wait(200))
            {
                if (_stopEvent.WaitOne(0))
                {
                    return;
                }
            }
            if (_videoUrls == null)
            {
                _stopEvent.Set();
            }
        }

        private void InitializeGrids()
        {
            FormHelper.ControlInvoker(_videoStageGrid, stageGrid =>
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
            FormHelper.ControlInvoker(_videoStageStatsGrid, statsGrid =>
            {
                var a = GetVideoStageStatsRow().GetAllParams();
                statsGrid.Rows.Add(a);
            });
        }

        private void StopDownloading()
        {
            if (_stopEvent == null || _stopEvent.WaitOne(0))
            {
                return;
            }
            FormHelper.ControlInvoker(startBtn, control =>
            {
                control.Enabled = false;
            });
            _stopEvent.Set();

            _launcher?.Stop();
            _previewLauncher?.Stop();
        }


        private DownloaderConfigs GetDownloaderConfigs()
        {
            return new DownloaderConfigs()
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
        }

        private VideoStageStats GetVideoStageStatsRow()
        {
            var stats = new VideoStageStats { All = _videoUrls.Count };
            foreach (var video in _videosProgress)
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
                var videoUrls = YouTubeHelper.GetVideosUrls(_currentUrl, GetDownloaderConfigs().YouTubeApiKey).ToList();
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
            while (!_launcher.Wait(1000))
            {
                FormHelper.ControlInvoker(_videoStageGrid, stageGrid =>
                {
                    foreach (var row in stageGrid.Rows)
                    {
                        var url = row.Cells["url"].Value.ToString();
                        if (!_videosProgress.ContainsKey(url) || !_videosProgress[url].IsStartedDownloadiong())
                        {
                            continue;
                        }
                        row.Cells["progress"].Value = _videosProgress[url].Progress;
                        row.Cells["stage"].Value = _videosProgress[url].Stage;
                        row.Cells["title"].Value = _videosProgress[url].Title;
                        row.Cells["video_types"].Value = _videosProgress[url].SelectedVideoType;
                    }
                });
                FormHelper.ControlInvoker(_videoStageStatsGrid, stageGrid =>
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

        private void PreviewGridUpdate()
        {
            while (!_previewLauncher.Wait(1000))
            {
                FormHelper.ControlInvoker(_videoStageGrid, stageGrid =>
                {
                    foreach (var row in stageGrid.Rows)
                    {
                        var url = row.Cells["url"].Value.ToString();
                        if (!_videosProgress.ContainsKey(url) || _videosProgress[url].PossibleVideoTypes == null)
                        {
                            continue;
                        }
                        //TODO: 1000 milis may prevent to update the grid after stopping
                        if (row.Cells["video_types"].Value == null && _videosProgress[url].PossibleVideoTypes != null)
                        {
                            var possibleVideoTypes = _videosProgress[url].PossibleVideoTypes;
                            var selectedVideoType = GetTypeByPriority(possibleVideoTypes.ToList());

                            row.Cells["video_types"].Value = selectedVideoType;
                            _videosProgress[url].SelectedVideoType = selectedVideoType;
                        }
                        row.Cells["title"].Value = _videosProgress[url].Title;
                    }
                });
            }
        }

        private VideoType GetTypeByPriority(ICollection<VideoType> possibleTypes)
        {
            var i = videoTypesDropDown.SelectedIndex;
            var selectedVideoType = VideoType.AvailabledVideoTypes[i];
            if (possibleTypes.Contains(selectedVideoType))
            {
                return selectedVideoType;
            }
            //Find videos with higher options
            var optionalVideoTypes = VideoType.AvailabledVideoTypes.ToList().GetRange(0, i);
            optionalVideoTypes.Reverse();

            selectedVideoType = optionalVideoTypes.FirstOrDefault(possibleTypes.Contains);
            if (selectedVideoType != null)
            {
                return selectedVideoType;
            }
            //Find videos with lower options
            optionalVideoTypes = VideoType.AvailabledVideoTypes.ToList().GetRange(i + 1, VideoType.AvailabledVideoTypes.Count - i - 1);
            selectedVideoType = optionalVideoTypes.FirstOrDefault(possibleTypes.Contains);
            return selectedVideoType; // May return null
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

        private void videoStageGrid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (!(e.CellElement.RowInfo is GridViewDataRowInfo dataRow))
            {
                return;
            }

            var urlColumn = dataRow.Cells["url"].Value.ToString();
            var stageColumn = dataRow.Cells["stage"].Value;
            if (e.Column.Name == "stage" && stageColumn != null)
            {
                e.CellElement.DrawFill = true;
                var color = StageColorFactory(_videosProgress[urlColumn].Stage);
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
        }


        private void videoStageGrid_CellClick(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.Column.Name == "video_types")
            {
                var url = e.Row.Cells["url"].Value.ToString();
                if (e.Column is GridViewComboBoxColumn comboBoxColumn)
                {
                    comboBoxColumn.DataSource = _videosProgress[url].PossibleVideoTypes.Select(v => v.ToString());
                }
            }
        }


        private void videoStageGrid_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.Column.Name == "video_types")
            {
                var url = e.Row.Cells["url"].Value.ToString();
                if (e.Column is GridViewComboBoxColumn comboBoxColumn && _videosProgress[url].PossibleVideoTypes != null)
                {
                    _videosProgress[url].SelectedVideoType = _videosProgress[url].PossibleVideoTypes
                        .FirstOrDefault(v => v.ToString() == e.Value.ToString());
                    comboBoxColumn.DataSource = _videosProgress[url].PossibleVideoTypes;
                }
            }
        }


        private void ProgressBarUpdate()
        {
            const int waitingTimeout = 500;
            IsWaitingBar(true);
            while (!_stopEvent.WaitOne(waitingTimeout) && (_launcher == null || (_launcher != null && !_launcher.IsAlive)))
            {
                FormHelper.ControlInvoker(waitingBar, bar => { bar.Text = "Trying to get all video urls"; });
            }
            IsWaitingBar(false);
            if (_launcher == null || _stopEvent.WaitOne(0))
            {
                return;
            }
            while (!_launcher.Wait(0) && !_stopEvent.WaitOne(waitingTimeout))
            {
                FormHelper.ControlInvoker(downloadProgressBar, bar =>
                {
                    var sum = _videosProgress.Values.Sum(v =>
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

            if (!_launcher.Wait(0))
            {
                FormHelper.ControlInvoker(waitingBar, bar => bar.Text = "Stopping");
                IsWaitingBar(true);
                while (!_launcher.Wait(waitingTimeout))
                {
                }
            }

            if (_stopEvent.WaitOne(0))
            {
                IsWaitingBar(true);
                FormHelper.ControlInvoker(waitingBar, bar =>
                {
                    bar.WaitingIndicatorSize = new Size(0, 100);
                    bar.Text = "Stopped";
                });
            }
            else
            {
                FormHelper.ControlInvoker(downloadProgressBar, bar =>
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
                FormHelper.ControlInvoker(downloadProgressBar, bar => bar.Visible = false);
                FormHelper.ControlInvoker(waitingBar, bar =>
                {
                    bar.Visible = true;
                    bar.StartWaiting();
                    bar.WaitingIndicatorSize = new Size(100, 100);
                });
            }
            else
            {
                FormHelper.ControlInvoker(downloadProgressBar, bar => bar.Visible = true);
                FormHelper.ControlInvoker(waitingBar, bar =>
                {
                    bar.Visible = false;
                    bar.StopWaiting();
                    bar.WaitingIndicatorSize = new Size(0, 100);
                });
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