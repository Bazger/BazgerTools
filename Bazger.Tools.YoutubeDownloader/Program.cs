using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bazger.Tools.YouTubeDownloader.Core;
using Bazger.Tools.YouTubeDownloader.Core.Converters;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using Bazger.Tools.YouTubeDownloader.Core.WebSites;
using NLog;

namespace Bazger.Tools.YouTubeDownloader
{
    internal static class Program
    {
        private static readonly Logger Log = LogManager.GetLogger("Console");

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
                var urlProblemVideos = new HashSet<string>();
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
                    PrintConsoleLogWithRetries($"{url} - {_videosProgress[url].Progress}%", url);
                }
                Console.WriteLine("\n-------------Video status info---------------------");
                Console.WriteLine($"Completed: { completedVideos.Count}/{_videoUrls.Count} Download Errors:{errorVideos.Count} Problem url:{urlProblemVideos.Count} Downloading:{inProgressVideos.Count} In Waiting Queue: {waitingToConvertVideos.Count} Converting: {convertingVideos.Count}");
                Console.WriteLine("\n---------------Threads info------------------------");
                Console.WriteLine("Donwload threads {0}/{1} Converters threads {2}/{3}",
                    _downloaderThreads.Count( c=> c.IsAlive), _downloaderThreads.Count,
                   _converterThreads.Count(c => c.IsAlive), _converterThreads.Count);
            }
            Console.WriteLine("\n---------------------------------------------------\n");
        }

        private static void ManageServiceThreads()
        {
            _videosProgress = new ConcurrentDictionary<string, VideoProgressMetadata>();

            var waitingForDownload = new BlockingCollection<string>(new ConcurrentQueue<string>(_videoUrls/*.GetRange(0,2)*/));
            var waitingForConvertion = new BlockingCollection<VideoProperties>();

            _downloaderThreads = new List<DownloaderThread>();
            _converterThreads = new List<ConverterThread>();

            _stopManagerEvent = new AutoResetEvent(false);

            IWebSiteDownloaderProxy website = new YouTubeProxy();
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
                IAudioConverterProxy converterProxy = new FFmpegConverterProxy();
                for (var i = 0; i < Configs.ConvertersCount; i++)
                {
                    _converterThreads.Add(new ConverterThread($"Converter {i}", converterProxy, waitingForConvertion,
                        _videosProgress, Configs.ConvertionFormat, _downloaderThreads));
                }

                Log.Info("Starting converterProxy threads");
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
                Log.Info("{0} converterProxy threads was started", Configs.ConvertersCount);
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
                Log.Error("there is a problem to read a journal file. May be journal format is illegal");
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

        private static void PrintConsoleLogWithRetries(string showStr, string video)
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
                    Log.Info("Stop converterProxy threads threads");
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
    }
}
