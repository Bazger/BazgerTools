using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.SessionState;
using System.Xml.Linq;
using Bazger.Tools.YouTubeDownloader.Converters;
using Bazger.Tools.YouTubeDownloader.Model;
using Bazger.Tools.YouTubeDownloader.Utility;
using Bazger.Tools.YouTubeDownloader.WebSites;
using NLog;
using YoutubeExtractor;

namespace Bazger.Tools.YouTubeDownloader
{
    internal static class Program
    {
        private static readonly Logger Log = LogManager.GetLogger("Console");

        private static volatile bool _isFinished;
        private static ConcurrentDictionary<string, VideoProgressMetadata> _videosProgress;
        public static readonly DownloaderConfigs Configs = DownloaderConfigs.GetConfig();

        private static List<string> _videoUrls;

        private static List<DownloaderThread> _downloaderThreads;
        private static List<ConverterThread> _converterThreads;

        private static AutoResetEvent _stopUiEvent;
        private static AutoResetEvent _stopManagerEvent;

        private static Thread _managerThread;
        private static Thread _uiThread;



        public static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            Directory.CreateDirectory(Configs.SaveDir);
            try
            {
                Log.Info("Trying to get all video urls");
                _videoUrls = YouTubeHelper.GetVideosUrls(Configs.DownloadUrl).ToList();
                if (!_videoUrls.Any())
                {
                    Log.Error("Your playlist or video url not contains video or null");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Check your dowload url or Api key, there are may be incorrect \n" + ex);
                return;
            }

            if (Configs.ReadFromJournal)
            {
                Log.Info("Reading from journal");
                _videoUrls = RemoveDownloadedVideos(_videoUrls).ToList();
            }

            _stopUiEvent = new AutoResetEvent(false);

            _managerThread = new Thread(ManageServiceThreads) { Name = "Manager" };
            _managerThread.Start();

            _uiThread = new Thread(UiDraw) { Name = "UI" };
            _uiThread.Start();

            //Console.CancelKeyPress += ConsoleCancelKeyPress;
        }

        private static void UiDraw()
        {
            _stopUiEvent.WaitOne();
            var drawLastTime = false;
            while (!drawLastTime)
            {
                if (_stopUiEvent.WaitOne(1000))
                {
                    drawLastTime = true;
                }
                Console.Clear();
                var inProgressVideos = new HashSet<string>();
                var errorVideos = new HashSet<string>();
                var waitingToConvertVideos = new HashSet<string>();
                var convertingVideos = new HashSet<string>();
                var completedVideos = new HashSet<string>();
                foreach (var video in _videosProgress)
                {
                    switch (video.Value.Stage)
                    {
                        case VideoProgressStage.Error:
                            errorVideos.Add(video.Key);
                            break;
                        case VideoProgressStage.Converting:
                            convertingVideos.Add(video.Key);
                            break;
                        case VideoProgressStage.WaitingToConvertion:
                            waitingToConvertVideos.Add(video.Key);
                            break;
                        case VideoProgressStage.Completed:
                            completedVideos.Add(video.Key);
                            break;
                        default:
                            inProgressVideos.Add(video.Key);
                            break;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var url in completedVideos)
                {
                    PrintConsoleLog($"{url} - Completed!", url);
                }
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var url in errorVideos)
                {
                    PrintConsoleLog($"{url} - Error!", url);
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var url in convertingVideos)
                {
                    Console.WriteLine("{0} - Converting...", url);
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var url in waitingToConvertVideos)
                {
                    Console.WriteLine("{0} - Waiting to convertion", url);
                }
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var url in inProgressVideos)
                {
                    PrintConsoleLog($"{url} - {_videosProgress[url].Progress}%", url);
                }
                Console.WriteLine("\n-------------Video status info---------------------");
                Console.WriteLine("Completed: {0}/{1} Errors:{2} Downloading:{3} In Waiting Queue: {4} Converting: {5}",
                    completedVideos.Count, _videoUrls.Count, errorVideos.Count, inProgressVideos.Count, waitingToConvertVideos.Count,
                    convertingVideos.Count);
                Console.WriteLine("\n---------------Threads info------------------------");
                Console.WriteLine("Donwload threads {0}/{1} Converters threads {2}/{3}",
                    _downloaderThreads.Count(c => c.IsAlive), _downloaderThreads.Count,
                    _converterThreads.Count(c => c.IsAlive), Configs.ConvertersCount);
            }
            Console.WriteLine("\n---------------------------------------------------\n");
        }

        private static void ManageServiceThreads()
        {
            _videosProgress = new ConcurrentDictionary<string, VideoProgressMetadata>();

            var waitingForDownload = new BlockingCollection<string>(new ConcurrentQueue<string>(_videoUrls/*.GetRange(0,2)*/));
            var waitingForConvertion = new BlockingCollection<Tuple<string, string>>();

            _downloaderThreads = new List<DownloaderThread>();
            _converterThreads = new List<ConverterThread>();

            _stopManagerEvent = new AutoResetEvent(false);

            IWebSiteDownloader website = new YouTube();
            for (var i = 0; i < Configs.ParallelDownloadsCount; i++)
            {
                _downloaderThreads.Add(new DownloaderThread($"Downloader {i}", website, _videosProgress, Configs.SaveDir, waitingForDownload, waitingForConvertion, Configs.ConverterEnabled));
            }

            Log.Info("Starting downloader threads");
            while (!_stopManagerEvent.WaitOne(200))
            {
                foreach (var service in _downloaderThreads.Where(c => !c.IsAlive && c.IsEnabled))
                    try
                    {
                        Log.Info("Starting service ({0})", service.Name);
                        service.Start();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error starting service ({0}): {1}", service.Name, ex);
                    }
                _stopManagerEvent.Set();
            }
            Log.Info("{0} downloading threads was started", Configs.ParallelDownloadsCount);

            if (Configs.ConverterEnabled)
            {
                IAudioConverter converter = new FFmpegConverter();
                for (var i = 0; i < Configs.ConvertersCount; i++)
                {
                    _converterThreads.Add(new ConverterThread($"Converter {i}", converter, waitingForConvertion,
                        _videosProgress, Configs.ConvertionFormat, _downloaderThreads));
                }

                Log.Info("Starting converter threads");
                while (!_stopManagerEvent.WaitOne(200))
                {
                    foreach (var service in _converterThreads.Where(c => !c.IsAlive && c.IsEnabled))
                        try
                        {
                            Log.Info("Starting service ({0})", service.Name);
                            service.Start();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error starting service ({0}): {1}", service.Name, ex);
                        }
                    _stopManagerEvent.Set();
                }
                Log.Info("{0} converter threads was started", Configs.ConvertersCount);
            }

            _stopUiEvent.Set();
            while (!_stopManagerEvent.WaitOne(1000))
            {
                if (_downloaderThreads.Count(c => c.IsAlive) != 0 || _converterThreads.Count(c => c.IsAlive) != 0)
                {
                    continue;
                }
                _stopUiEvent.Set();
                _stopManagerEvent.Set();
                if (!Configs.WriteToJournal)
                {
                    continue;
                }

                var stopWriteToJournal = new AutoResetEvent(false);
                while (!stopWriteToJournal.WaitOne(1000))
                {
                    if (_uiThread.IsAlive)
                    {
                        continue;
                    }
                    Log.Info("Writing to journal");
                    WriteToJournal();
                    Log.Info("Writing succeeded");
                    stopWriteToJournal.Set();
                }

            }
        }

        private static IEnumerable<string> RemoveDownloadedVideos(List<string> videoUrls)
        {
            if (!File.Exists(Configs.JournalFileName))
            {
                Log.Warn("Journal file doen't exist");
                return videoUrls;
            }
            try
            {
                var downloadedVideos = SerDeUtils.DeserializeJsonFile<HashSet<string>>(Configs.JournalFileName);
                return videoUrls.Where(url => !downloadedVideos.Contains(url));
            }
            catch (Exception)
            {
                Log.Error("there is a problem to write to journal file. May be journal format is illegal");
            }
            return videoUrls;
        }

        private static void WriteToJournal()
        {
            try
            {

                if (!File.Exists(Configs.JournalFileName))
                {
                    Log.Warn("Journal file doen't exist. Creating new one");
                    SerDeUtils.SerializeToJsonFile(new HashSet<string>(), Configs.JournalFileName);
                }
                var downloadedVideos = SerDeUtils.DeserializeJsonFile<HashSet<string>>(Configs.JournalFileName);
                foreach (
                    var videoUrl in
                        _videoUrls.Where(
                            url =>
                                _videosProgress.ContainsKey(url) &&
                                _videosProgress[url].Stage == VideoProgressStage.Completed))
                {
                    downloadedVideos.Add(videoUrl);
                }
                SerDeUtils.SerializeToJsonFile(downloadedVideos, Configs.JournalFileName);
            }
            catch (Exception)
            {
                Log.Error("there is a problem to write to journal file. May be journal format is illegal");
            }
        }

        private static void PrintConsoleLog(string showStr, string video)
        {
            var tryStr = "";
            if (_videosProgress[video].Retries > 0)
            {
                tryStr = " | Retries: " + _videosProgress[video].Retries;
            }
            Console.WriteLine(showStr + tryStr);
        }

        private static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey == ConsoleSpecialKey.ControlC)
            {
                Log.Info("Stop downloader threads");
                foreach (var service in _downloaderThreads.Where(c => !c.IsAlive && c.IsEnabled))
                {
                    Log.Info("Stopping service ({0})", service.Name);
                    service.Stop();
                }

                if (_converterThreads.Any())
                {
                    Log.Info("Stop converter threads threads");
                    foreach (var service in _converterThreads.Where(c => !c.IsAlive && c.IsEnabled))
                    {
                        Log.Info("Stopping service ({0})", service.Name);
                        service.Stop();
                    }
                }

                //TODO: Close Manager and UI threads

                e.Cancel = true;
            }
        }

        public static void Main_v1(string[] args)
        {
            Directory.CreateDirectory(Configs.SaveDir);

            List<string> videoUrls;
            try
            {
                Console.WriteLine("Trying to get all video urls");
                videoUrls = YouTubeHelper.GetVideosUrls(Configs.DownloadUrl).ToList();
                if (!videoUrls.Any())
                {
                    Console.WriteLine("Playlist is empty, try again");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Check your dowload url or Api key, there are may be incorrect \n" + ex);
                return;
            }


            _videosProgress = new ConcurrentDictionary<string, VideoProgressMetadata>();

            IWebSiteDownloader downloader = new YouTube();
            //downloader.InitVideosProgress(_videosProgress);
            IAudioConverter converter = new FFmpegConverter();

            new Thread(() =>
            {
                Parallel.ForEach(videoUrls, new ParallelOptions() { MaxDegreeOfParallelism = Configs.ParallelDownloadsCount }, url =>
                {
                    string videoPath = null;
                    try
                    {
                        _videosProgress.TryAdd(url, new VideoProgressMetadata() { Stage = VideoProgressStage.Downloading, Progress = 0 });
                        videoPath = downloader.Download(url, Configs.SaveDir, _videosProgress[url]);
                        if (_videosProgress[url].Stage == VideoProgressStage.Exist)
                        {
                            return;
                        }
                        if (Configs.ConverterEnabled)
                        {
                            _videosProgress[url].Stage = VideoProgressStage.Converting;
                            converter.Convert(videoPath, Configs.ConvertionFormat);
                            File.Delete(videoPath);
                        }
                        _videosProgress[url].Stage = VideoProgressStage.Completed;
                    }
                    catch (Exception ex)
                    {
                        if (videoPath != null && File.Exists(videoPath))
                        {
                            File.Delete(videoPath);
                        }
                        _videosProgress[url].ErrorArgs = ex.ToString();
                        _videosProgress[url].Stage = VideoProgressStage.Error;
                    }
                });
                _isFinished = true;
            }).Start();

            while (!_isFinished)
            {
                Console.Clear();
                var inProgressVideos = new HashSet<string>();
                var errorVideos = new HashSet<string>();
                var convertingVideos = new HashSet<string>();
                var completedVideos = new HashSet<string>();
                foreach (var video in _videosProgress)
                {
                    switch (video.Value.Stage)
                    {
                        case VideoProgressStage.Error:
                            errorVideos.Add(video.Key);
                            break;
                        case VideoProgressStage.Converting:
                            convertingVideos.Add(video.Key);
                            break;
                        case VideoProgressStage.Completed:
                            completedVideos.Add(video.Key);
                            break;
                        default:
                            inProgressVideos.Add(video.Key);
                            break;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var video in completedVideos)
                {
                    Console.WriteLine("{0} - Completed!", video);
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var video in convertingVideos)
                {
                    Console.WriteLine("{0} - Converting...", video);
                }
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var video in errorVideos)
                {
                    Console.WriteLine("{0} - Error!", video);
                }
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var video in inProgressVideos)
                {
                    Console.WriteLine("{0} - {1}%", video, _videosProgress[video].Progress);
                }
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("Completed: {0}/{1} Errors:{2} Downloading:{3} Converting: {4}",
                    completedVideos.Count, videoUrls.Count, errorVideos.Count, inProgressVideos.Count,
                    convertingVideos.Count);
                Thread.Sleep(1000);
            }
        }

    }
}
