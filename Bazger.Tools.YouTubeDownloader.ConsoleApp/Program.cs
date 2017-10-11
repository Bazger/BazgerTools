using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.ConsoleApp
{
    public static class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly DownloaderConfigs Configs = DownloaderConfigs.GetConfig();

        private static List<string> _videoUrls;
        private static Launcher _launcher;
        private static Thread _uiThread;
        private static AutoResetEvent _stopUiEvent;

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

            _launcher = new Launcher(_videoUrls, Configs);
            _launcher.Start();

            _stopUiEvent = new AutoResetEvent(true);

            _uiThread = new Thread(UiDraw) { Name = "UI" };
            _uiThread.Start();

            Console.CancelKeyPress += ConsoleCancelKeyPress;
            //Thread.Sleep(30000);
            //_launcher.Stop();
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
                var urlProblemVideos = new HashSet<string>();
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
                        default:
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
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var url in inProgressVideos)
                {
                    PrintConsoleLogWithRetries($"{url} - {_launcher.VideosProgress[url].Progress}%", url);
                }
                Console.WriteLine("\n-------------Video status info---------------------");
                Console.WriteLine($"Completed: { completedVideos.Count}/{_videoUrls.Count} Download Errors:{errorVideos.Count} Problem url:{urlProblemVideos.Count} Downloading:{inProgressVideos.Count} In Waiting Queue: {waitingToConvertVideos.Count} Converting: {convertingVideos.Count}");
                Console.WriteLine("\n---------------Threads info------------------------");
                Console.WriteLine("Donwload threads {0}/{1} Converters threads {2}/{3}",
                    _launcher.GetAliveDownloadersCount(), _launcher.GetAllDownloadersCount(),
                   _launcher.GetAliveConvertersCount(), _launcher.GetAllConvertersCount());
            }
            Console.WriteLine("\n---------------------------------------------------\n");
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
            _launcher.Stop();
            e.Cancel = true;
        }
    }
}
