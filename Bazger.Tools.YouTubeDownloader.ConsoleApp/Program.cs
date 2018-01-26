using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bazger.Tools.YouTubeDownloader.Core;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;
using YoutubeExtractor;

namespace Bazger.Tools.YouTubeDownloader.ConsoleApp
{
    public static class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly DownloaderConfigs Configs = DownloaderConfigs.GetConfig();

        private static List<string> _videoUrls;
        private static MainLauncher _launcher;
        private static Task _uiThread;
        private static AutoResetEvent _stopUiEvent;
        private static bool _onStopping;
        private static bool _launcherStopped;
        private static int _stopAnimationStage;

        public static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            try
            {
                Log.Info("Trying to get all video urls");
                _videoUrls = YouTubeHelper.GetVideosUrls(
                    System.Configuration.ConfigurationManager.AppSettings["downloadUrl"], Configs.YouTubeApiKey
                    ).ToList();
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
                var downloadedVideos = FileHelper.ReadFromJournal(Configs.JournalFilePath);
                if (downloadedVideos != null)
                {
                    //Get the dif from downloaded videos and all videos
                    _videoUrls = _videoUrls.Except(downloadedVideos).ToList();
                    Log.Info("Journal file loaded successfully");
                }
                if (!_videoUrls.Any())
                {
                    Log.Warn("There are no urls to download, all of them has been downloaded yet");
                    return;
                }
            }

            _launcher = new MainLauncher(_videoUrls, Configs);
            _launcher.Start();

            _stopUiEvent = new AutoResetEvent(true);

            _uiThread = new Task(UiDraw);
            _uiThread.Start();

            Console.CancelKeyPress += ConsoleCancelKeyPress;

            Task.WaitAll(_uiThread);

            if (Configs.WriteToJournal)
            {
                Log.Info("Writing to journal");
                FileHelper.WriteToJournal(Configs.JournalFilePath, _launcher.VideosProgress.Values
                    .Where(v => v.Stage == VideoProgressStage.Completed)
                    .Select(v => v.Url)
                    .ToList());
                Log.Info("Writing succeeded");
            }
        }

        private static void UiDraw()
        {
            Thread.CurrentThread.Name = "UI";
            _stopUiEvent.WaitOne();
            var drawLastTime = false;
            while (!drawLastTime)
            {
                if (_stopUiEvent.WaitOne(1000) || _launcher.Wait(0))
                {
                    drawLastTime = true;
                }
                Console.Clear();
                var inProgressVideos = new HashSet<string>();
                var errorVideos = new HashSet<string>();
                var waitingToConvertVideos = new HashSet<string>();
                var convertingVideos = new HashSet<string>();
                var completedVideos = new HashSet<string>();
                var urlProblemVideos = new HashSet<string>();
                var movingVideos = new HashSet<string>();
                foreach (var video in _launcher.VideosProgress)
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
                        case VideoProgressStage.VideoUrlProblem:
                            urlProblemVideos.Add(video.Key);
                            break;
                        case VideoProgressStage.Moving:
                            movingVideos.Add(video.Key);
                            break;
                        case VideoProgressStage.Downloading:
                            inProgressVideos.Add(video.Key);
                            break;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var url in completedVideos)
                {
                    PrintConsoleLogWithRetries($"{url} - Completed!", url);
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                foreach (var url in urlProblemVideos)
                {
                    Console.WriteLine("{0} - Url Problem!", url);
                }
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var url in errorVideos)
                {
                    PrintConsoleLogWithRetries($"{url} - Error!", url);
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
                foreach (var url in movingVideos)
                {
                    Console.WriteLine("{0} - Moving...", url);
                }
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var url in inProgressVideos)
                {
                    PrintConsoleLogWithRetries($"{url} - {_launcher.VideosProgress[url].Progress}%", url);
                }
                Console.WriteLine("\n-------------Video status info---------------------");
                Console.WriteLine($"Completed:{ completedVideos.Count}/{_videoUrls.Count} Download Errors:{errorVideos.Count} Problem url:{urlProblemVideos.Count}\nDownloading:{inProgressVideos.Count} Waiting:{waitingToConvertVideos.Count} Converting:{convertingVideos.Count} Moving:{movingVideos.Count}");
                Console.WriteLine("\n---------------Threads info------------------------");
                Console.WriteLine("Donwload threads {0}/{1} Converters threads {2}/{3} File mover threads {4}/{5}",
                    _launcher.GetAliveDownloadersCount(), _launcher.GetAllDownloadersCount(),
                   _launcher.GetAliveConvertersCount(), _launcher.GetAllConvertersCount(), _launcher.GetAliveFileMoversCount(), _launcher.GetAllFileMoversCount());
                Console.WriteLine("\n---------------------------------------------------");
                if (_launcherStopped)
                {
                    Console.Write("Successfully stoped");
                    Console.WriteLine("\n---------------------------------------------------\n");
                }
                else if (_onStopping)
                {
                    Console.Write("Stopping");
                    for (int i = 0; i <= _stopAnimationStage % 3; i++)
                    {
                        Console.Write(".");
                    }
                    _stopAnimationStage++;
                    Console.WriteLine();
                    Console.WriteLine("\n---------------------------------------------------\n");
                }
            }
        }

        private static void PrintConsoleLogWithRetries(string showStr, string video)
        {
            var tryStr = "";
            if (_launcher.VideosProgress[video].Retries > 0)
            {
                tryStr = " | Retries: " + _launcher.VideosProgress[video].Retries;
            }
            Console.WriteLine(showStr + tryStr);
        }


        private static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey != ConsoleSpecialKey.ControlC)
            {
                return;
            }
            Thread.CurrentThread.Name = "ConsoleCancelKeyPress";
            _onStopping = true;
            _launcher.Stop();
            _launcherStopped = true;
            _stopUiEvent.Set();
            e.Cancel = true;
        }
    }
}
