using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
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
        public string LoggerName => this.GetType().FullName;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private ManualResetEvent _stopEvent;
        private ManualResetEvent _stoppedEvent;

        private MainLauncher _launcher;
        private PreviewLauncher _previewLauncher;
        private Thread _gridUpdate;
        private Thread _progressBarUpdate;
        private Thread _workerThread;

        private string _currentUrl;
        private List<string> _videoUrls;

        private RadGridView _videoStageGrid;
        private RadGridView _videoStageStatsGrid;
        private bool _isStarted;
        private bool _isPreview;
        private IDictionary<string, VideoProgressMetadata> _videosProgress;
        private IDictionary<string, VideoProgressMetadata> _videoProgressForRevert;
        private List<string> _savedDownloadsFolderPaths;
        private List<string> _savedJournalFilePaths;


        private const string VideoTypesColumn = "video_types";
        private const string ProgressColumn = "progress";
        private const string StageColumn = "stage";
        private const string TitleColumn = "title";
        private const string UrlColumn = "url";

        public YouTubeDownloaderControl()
        {
            InitializeComponent();

            _isStarted = false;
            _isPreview = false;

            videoTypesDropDown.DataSource = VideoType.AvailabledVideoTypes;
            startBtn.DropDownButtonElement.ActionButton.Click += MainBtn_Click;
        }

        public void IntializeControl(MainForm mainForm)
        {
            _mainForm = mainForm;

            _videoStageGrid = _mainForm.VideoStageGrid;
            _videoStageGrid.CellFormatting += VideoStageGrid_CellFormatting;
            _videoStageGrid.CellValueChanged += VideoStageGrid_CellValueChanged;
            _videoStageGrid.CellBeginEdit += VideoStageGrid_CellBeginEdit;
            _videoStageGrid.CellDoubleClick += VideoStageGrid_CellDoubleClick;

            _videoStageStatsGrid = _mainForm.VideoStageStatsGrid;

            _mainForm.AddRuleToPageLoggerTarget("Bazger.Tools.YouTubeDownloader.Core.*", LogLevel.Info, LoggerName);
        }

        public void LoadState(IControlState controlState)
        {
            if (!(controlState is YouTubeDownloaderControlState state)) { return; }

            if (!string.IsNullOrEmpty(state.Url) && ValidateUrl(state.Url))
            {
                urlTxtBox.Text = state.Url;
            }
            else
            {
                startBtn.Enabled = false;
            }
            converterThreadsSpin.Value = state.ConvertersThreadSpin;
            convertionFormatsDropDownList.SelectedIndex = state.ConvertionFormat;
            downloaderThreadsSpin.Value = state.DownloadersThreadSpin;
            previewThreadSpin.Value = state.PreviewThreadSpin;
            overwriteFilesChkBox.Checked = state.IsOverwriteChecked;
            readFromJournalChkBox.Checked = state.IsReadFromJournalChecked;
            writeToJournalChkBox.Checked = state.IsWriteToJournalCheked;
            convertionEnabledChkBox.Checked = state.IsConversionChecked;
            journalFileDropDown.DataSource = _savedJournalFilePaths = state.SavedJournalFilePaths ?? new List<string>();
            downloadsFolderDropDown.DataSource = _savedDownloadsFolderPaths = state.SavedDownloadsFolderPaths ?? new List<string>();
            if (!string.IsNullOrEmpty(state.DownloadsFolderPath))
            {
                downloadsFolderDropDown.Text = state.DownloadsFolderPath;
            }
            if (!string.IsNullOrEmpty(state.JournalFilePath))
            {
                journalFileDropDown.Text = state.JournalFilePath;
            }
            videoTypesDropDown.SelectedIndex = state.VideoTypeId;
        }

        public IControlState SaveState()
        {
            return new YouTubeDownloaderControlState
            {
                Url = ValidateUrl(urlTxtBox.Text) ? urlTxtBox.Text : null,
                DownloadersThreadSpin = (int)downloaderThreadsSpin.Value,
                ConvertersThreadSpin = (int)converterThreadsSpin.Value,
                PreviewThreadSpin = (int)previewThreadSpin.Value,
                IsConversionChecked = convertionEnabledChkBox.Checked,
                ConvertionFormat = convertionFormatsDropDownList.SelectedIndex,
                DownloadsFolderPath = downloadsFolderDropDown.Text,
                SavedDownloadsFolderPaths = (List<string>)downloadsFolderDropDown.DataSource,
                IsOverwriteChecked = overwriteFilesChkBox.Checked,
                IsReadFromJournalChecked = readFromJournalChkBox.Checked,
                IsWriteToJournalCheked = writeToJournalChkBox.Checked,
                JournalFilePath = journalFileDropDown.Text,
                SavedJournalFilePaths = (List<string>)journalFileDropDown.DataSource,
                VideoTypeId = videoTypesDropDown.SelectedIndex
            };
        }

        public void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _stopEvent?.Set();
            _workerThread?.Abort();
            _gridUpdate?.Abort();
            _progressBarUpdate?.Abort();
            _launcher?.Abort();
            _previewLauncher?.Abort();
        }

        private void UrlTxtBox_Focus(object sender, EventArgs e)
        {
            if (urlTxtBox.Text == Resources.youtubeDonwloaderUrlInfo)
            {
                urlTxtBox.Text = null;
            }
        }

        private void UrlTxtBox_Leave(object sender, EventArgs e)
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

        private void MainBtn_Click(object sender, EventArgs e)
        {
            if (!_isStarted)
            {
                if (string.IsNullOrEmpty(downloadsFolderDropDown.Text))
                {
                    Log.Error("Downloads path can't be null");
                    return;
                }
                if (!_isPreview)
                {
                    StartNewOneBtn_Click(sender, e);
                }
                else
                {
                    ToWorkState();
                    _stopEvent = new ManualResetEvent(false);
                    _stoppedEvent = new ManualResetEvent(false);
                    _workerThread = new Thread(DownloadVideos) { Name = "DownloadsManager" };
                    _workerThread.Start();
                }
            }
            else
            {
                _workerThread = new Thread(StopDownloading) { Name = "StopManager" };
                _workerThread.Start();
            }
        }

        private void StartNewOneBtn_Click(object sender, EventArgs e)
        {
            _currentUrl = urlTxtBox.Text;
            if (!ValidateUrl(_currentUrl))
            {
                Log.Warn($"Entered url is not correct, try new one | url={_currentUrl}");
                return;
            }
            _isPreview = false;
            ToWorkState();
            _stopEvent = new ManualResetEvent(false);
            _stoppedEvent = new ManualResetEvent(false);
            _workerThread = new Thread(DownloadVideos) { Name = "DownloadsManager" };
            _workerThread.Start();
        }

        private void PreviewMenuItem_Click(object sender, EventArgs e)
        {
            _currentUrl = urlTxtBox.Text;
            if (!ValidateUrl(_currentUrl))
            {
                Log.Warn($"Entered url is not correct, try new one | url={_currentUrl}");
                return;
            }
            _isPreview = false;
            ToWorkState();
            _stopEvent = new ManualResetEvent(false);
            _stoppedEvent = new ManualResetEvent(false);
            _workerThread = new Thread(PreviewVideos) { Name = "PreviewManager" };
            _workerThread.Start();
        }

        private void BackToPreview_Click(object sender, EventArgs e)
        {
            _videosProgress = _videoProgressForRevert;
            _isPreview = true;
            RevertGrid();
            FormHelper.ControlInvoker(startBtn, control => control.Items.Remove(backToPreviewItem));
            ToIdleState();
        }

        private void ToIdleState()
        {
            _isStarted = false;
            FormHelper.ControlInvoker(urlTxtBox, control => { control.Enabled = true; });
            FormHelper.ControlInvoker(converterPnl, control => { control.Enabled = true; });
            FormHelper.ControlInvoker(pathsPnl, control => { control.Enabled = true; });
            FormHelper.ControlInvoker(threadPnl, control => { control.Enabled = true; });
            FormHelper.ControlInvoker(videoTypeSelectorPnl, control => { control.Enabled = true; });
            if (!_isPreview)
            {
                FormHelper.ControlInvoker(startBtn, control =>
                {
                    control.Text = Resources.startBtn_Start;
                    control.Enabled = true;
                    control.Items.Add(previewMenuItem);
                });
            }
            else
            {
                FormHelper.ControlInvoker(downloadProgressBar, bar =>
                {
                    bar.Value1 = 0;
                    bar.Text = "0%";
                });
                FormHelper.ControlInvoker(startBtn, control =>
                {
                    control.Text = Resources.startBtn_Download;
                    control.DefaultItem = mainMenuItem;
                    control.Enabled = true;
                    control.Items.AddRange(startNewOneMenuItem, previewMenuItem);
                });
                FormHelper.ControlInvoker(_videoStageGrid, control => { control.Columns[VideoTypesColumn].ReadOnly = false; });
            }
        }

        private void ToWorkState()
        {
            _isStarted = true;
            FormHelper.ControlInvoker(startBtn, control =>
            {
                control.Text = Resources.startBtn_Stop;
                control.Items.Clear();
            });
            FormHelper.ControlInvoker(converterPnl, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(urlTxtBox, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(pathsPnl, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(threadPnl, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(videoTypeSelectorPnl, control => { control.Enabled = false; });
            FormHelper.ControlInvoker(_videoStageGrid, control => { control.Columns[VideoTypesColumn].ReadOnly = true; });
            FormHelper.ControlInvoker(_videoStageStatsGrid, control => { control.Rows.Clear(); });

            FormHelper.ControlInvoker(downloadProgressBar, bar =>
            {
                bar.Value1 = 0;
                bar.Text = "0%";
            });

            if (!_isPreview)
            {
                FormHelper.ControlInvoker(_videoStageGrid, control => { control.Rows.Clear(); });
                FormHelper.ControlInvoker(downloadProgressBar, bar =>
                {
                    bar.Visible = false;
                });
                FormHelper.ControlInvoker(waitingBar, bar =>
                {
                    bar.Visible = true;
                    bar.Text = "Trying to get all video urls";
                });
            }
        }

        private void PreviewVideos()
        {
            _progressBarUpdate = new Thread(PreivewLauncherProgressBarUpdate) { Name = "ProgressBarUpdate" };
            _progressBarUpdate.Start();

            SavePaths();

            LoadAllVideoUrls();
            if (_stopEvent.WaitOne(0))
            {
                _stoppedEvent.Set();
                ToIdleState();
                return;
            }

            var config = GetDownloaderConfigs();
            GetVideoUrlsFromJournal(config.ReadFromJournal, config.JournalFilePath);
            if (_stopEvent.WaitOne(0))
            {
                _stoppedEvent.Set();
                ToIdleState();
                return;
            }

            _previewLauncher = new PreviewLauncher(_videoUrls, config.YouTubeVideoTypeId, config.PreviewThreadsCount);
            _previewLauncher.Start();

            _videosProgress = _previewLauncher.VideosProgress;

            InitializeStageGrid();
            InitializeStatsGrid();

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
            _stoppedEvent.Set();
            ToIdleState();
        }

        private void DownloadVideos()
        {
            DownloaderConfigs configs = GetDownloaderConfigs();
            SavePaths();
            if (!_isPreview)
            {
                _progressBarUpdate = new Thread(MainLauncherProgressBarUpdate) { Name = "ProgressBarUpdate" };
                _progressBarUpdate.Start();
                LoadAllVideoUrls();
                if (_stopEvent.WaitOne(0))
                {
                    _stoppedEvent.Set();
                    ToIdleState();
                    return;
                }
                GetVideoUrlsFromJournal(configs.ReadFromJournal, configs.JournalFilePath);
                if (_stopEvent.WaitOne(0))
                {
                    _stoppedEvent.Set();
                    ToIdleState();
                    return;
                }
                _launcher = new MainLauncher(_videoUrls, configs);
                _launcher.Start();
                _videosProgress = _launcher.VideosProgress;
                InitializeStageGrid();
                InitializeStatsGrid();
            }
            else
            {
                _videoProgressForRevert = _videosProgress;
                _launcher = new MainLauncher(_videosProgress, configs, _videoUrls);
                _launcher.Start();
                _videosProgress = _launcher.VideosProgress;

                _progressBarUpdate = new Thread(MainLauncherProgressBarUpdate) { Name = "ProgressBarUpdate" };
                _progressBarUpdate.Start();

                InitializeStatsGrid();
            }

            _gridUpdate = new Thread(GridUpdate) { Name = "GridUpdate" };
            _gridUpdate.Start();

            while (!_launcher.Wait(1000))
            {
                //Waiting for finishing or stop event call
            }

            if (configs.WriteToJournal)
            {
                Log.Info("Writing to journal");
                var videos = _videosProgress.Values.Where(v => v.Stage == VideoProgressStage.Completed).Select(v => v.Url).ToList();
                FileHelper.WriteToJournal(configs.JournalFilePath, videos);
                Log.Info($"Writing of {videos.Count} videos succeeded");
            }

            if (_stopEvent.WaitOne(0) && _isPreview)
            {
                _videosProgress = _videoProgressForRevert;
                RevertGrid();
            }
            else if (!_stopEvent.WaitOne(0))
            {
                if (_isPreview)
                {
                    FormHelper.ControlInvoker(startBtn, control => control.Items.Add(backToPreviewItem));
                    _isPreview = false;
                }
            }
            _stoppedEvent.Set();
            ToIdleState();
        }

        private void LoadAllVideoUrls()
        {
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

        private void SavePaths()
        {
            if (!string.IsNullOrEmpty(downloadsFolderDropDown.Text) && !_savedDownloadsFolderPaths.Contains(downloadsFolderDropDown.Text))
            {
                var text = downloadsFolderDropDown.Text;
                _savedDownloadsFolderPaths.Add(text);
                FormHelper.ControlInvoker(downloadsFolderDropDown, control =>
                {
                    control.DataSource = _savedDownloadsFolderPaths.ToList();
                    control.Text = text;
                    control.Update();
                });
            }
            if (!string.IsNullOrEmpty(journalFileDropDown.Text) && !_savedJournalFilePaths.Contains(journalFileDropDown.Text))
            {
                var text = journalFileDropDown.Text;
                _savedJournalFilePaths.Add(text);
                FormHelper.ControlInvoker(journalFileDropDown, control =>
                {
                    control.DataSource = _savedJournalFilePaths.ToList();
                    control.Text = text;
                    control.Update();
                });
            }
        }

        private void InitializeStageGrid()
        {
            FormHelper.ControlInvoker(_videoStageGrid, stageGrid =>
            {
                var i = 1;
                _videoUrls.ForEach(url =>
                {
                    stageGrid.Rows.Add(i++, url);
                });
                stageGrid.TableElement.RowScroller.ScrollToFirstRow();
            });

        }

        private void InitializeStatsGrid()
        {
            FormHelper.ControlInvoker(_videoStageStatsGrid, statsGrid =>
            {
                var firstRow = GetVideoStageStatsRow().GetAllParams();
                statsGrid.Rows.Add(firstRow);
            });
        }

        private void RevertGrid()
        {
            FormHelper.ControlInvoker(_videoStageGrid, stageGrid =>
            {
                foreach (var row in stageGrid.Rows)
                {
                    var url = row.Cells[UrlColumn].Value.ToString();
                    row.Cells[ProgressColumn].Value = null;
                    row.Cells[StageColumn].Value = _videosProgress[url].Stage;
                }
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

        private void GetVideoUrlsFromJournal(bool readFromJournal, string journalFilePath)
        {
            if (!readFromJournal)
            {
                return;
            }
            Log.Info("Reading from journal");
            var downloadedVideos = FileHelper.ReadFromJournal(journalFilePath);
            if (downloadedVideos != null)
            {
                //Get the dif from downloaded videos and all videos
                _videoUrls = _videoUrls.Except(downloadedVideos).ToList();
                Log.Info("Journal file loaded successfully");
            }
            if (!_videoUrls.Any())
            {
                Log.Warn("There are no urls to download, all of them has been downloaded yet");
                FormHelper.ControlInvoker(downloadProgressBar, bar =>
                {
                    bar.Value1 = 0;
                    bar.Text = Resources.downloadProgressBar_All_Of_Videos_Has_Been_Downloaded_Yet;
                });
                _stopEvent.Set();
            }
        }

        private DownloaderConfigs GetDownloaderConfigs()
        {
            return new DownloaderConfigs()
            {
                YouTubeApiKey = Resources.youtubeApiKey,
                SaveDir = downloadsFolderDropDown.Text,
                YouTubeVideoTypeId = VideoType.AvailabledVideoTypes[videoTypesDropDown.SelectedIndex].Id,
                ParallelDownloadsCount = (int)downloaderThreadsSpin.Value,
                ConvertersCount = (int)converterThreadsSpin.Value,
                PreviewThreadsCount = (int)previewThreadSpin.Value,
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
                        var url = row.Cells[UrlColumn].Value.ToString();
                        if (!_videosProgress.ContainsKey(url) || !_videosProgress[url].IsStartedDownloadiong())
                        {
                            continue;
                        }
                        row.Cells[ProgressColumn].Value = _videosProgress[url].Progress;
                        row.Cells[StageColumn].Value = _videosProgress[url].Stage;
                        row.Cells[TitleColumn].Value = _videosProgress[url].Title;
                        row.Cells[VideoTypesColumn].Value = _videosProgress[url].SelectedVideoType;
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
                PreviewGridInnerUpdate();
            }
            PreviewGridInnerUpdate();
        }

        private void PreviewGridInnerUpdate()
        {
            FormHelper.ControlInvoker(_videoStageGrid, stageGrid =>
            {
                foreach (var row in stageGrid.Rows)
                {
                    var url = row.Cells[UrlColumn].Value.ToString();
                    if (!_videosProgress.ContainsKey(url) || _videosProgress[url].Stage == VideoProgressStage.Idling)
                    {
                        continue;
                    }
                    if (row.Cells[VideoTypesColumn].Value == null && _videosProgress[url].SelectedVideoType != null)
                    {
                        row.Cells[VideoTypesColumn].Value = _videosProgress[url].SelectedVideoType.ToString();
                    }
                    row.Cells[TitleColumn].Value = _videosProgress[url].Title;
                    row.Cells[StageColumn].Value = _videosProgress[url]?.Stage;
                }
            });
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
                case VideoProgressStage.PreviewFound:
                    return Color.LightGray;
                case VideoProgressStage.GettingPreview:
                    return Color.WhiteSmoke;
                default:
                    return null;
            }
        }

        private void VideoStageGrid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (!(e.CellElement.RowInfo is GridViewDataRowInfo dataRow))
            {
                return;
            }

            var urlColumn = dataRow.Cells[UrlColumn].Value.ToString();
            var stageColumn = dataRow.Cells[StageColumn].Value;
            if (e.Column.Name == StageColumn && stageColumn != null)
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

        private void VideoStageGrid_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.Column.Name == VideoTypesColumn)
            {
                var url = e.Row.Cells[UrlColumn].Value.ToString();
                if (e.Column is GridViewComboBoxColumn comboBoxColumn)
                {
                    comboBoxColumn.DataSource = _videosProgress[url].PossibleVideoTypes?.Select(v => v.ToString());
                }
            }
        }

        private void VideoStageGrid_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name == VideoTypesColumn)
            {
                var url = e.Row.Cells[UrlColumn].Value.ToString();
                if (e.Column is GridViewComboBoxColumn comboBoxColumn && _videosProgress[url].PossibleVideoTypes != null)
                {
                    _videosProgress[url].SelectedVideoType = _videosProgress[url].PossibleVideoTypes
                        .FirstOrDefault(v => v.ToString() == e.Value.ToString());
                    comboBoxColumn.DataSource = _videosProgress[url].PossibleVideoTypes;
                }
            }
        }

        private void VideoStageGrid_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name == TitleColumn)
            {
                var url = e.Row.Cells[UrlColumn].Value.ToString();
                if (string.IsNullOrEmpty(url))
                {
                    return;
                }
                var videoFilePath = _videosProgress[url].VideoFilePath;
                if (videoFilePath == null || _videosProgress[url].Stage != VideoProgressStage.Completed)
                {
                    return;
                }
                Process.Start(videoFilePath);
            }
        }

        private void MainLauncherProgressBarUpdate()
        {
            const int waitingTimeout = 500;
            if (!_isPreview)
            {
                IsWaitingBar(true);
                while (!_stopEvent.WaitOne(waitingTimeout) &&
                       (_launcher == null || (_launcher != null && !_launcher.IsAlive)))
                {
                    FormHelper.ControlInvoker(waitingBar, bar => { bar.Text = "Trying to get all video urls"; });
                }
            }
            IsWaitingBar(false);
            if (_launcher == null || _stopEvent.WaitOne(0))
            {
                return;
            }
            while (!_launcher.Wait(waitingTimeout) && !_stopEvent.WaitOne(0))
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

        private void PreivewLauncherProgressBarUpdate()
        {
            const int waitingTimeout = 500;
            FormHelper.ControlInvoker(waitingBar, bar => { bar.Text = "Finding preview for all videos"; });
            IsWaitingBar(true);
            while (!_stopEvent.WaitOne(waitingTimeout) && (_previewLauncher == null || (_previewLauncher != null && _previewLauncher.IsAlive)))
            {
            }
            IsWaitingBar(false);
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
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private void UrlTxtBox_TextChanging(object sender, TextChangingEventArgs e)
        {
            startBtn.Enabled = !string.IsNullOrEmpty(e.NewValue);
        }

        private void GoToFolderBtn_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(downloadsFolderDropDown.Text))
            {
                Process.Start(downloadsFolderDropDown.Text);
            }
        }

        private void BrowseJournalBtn_Click(object sender, EventArgs e)
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

        private void BrowseDownloadsBtn_Click(object sender, EventArgs e)
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
    }
}