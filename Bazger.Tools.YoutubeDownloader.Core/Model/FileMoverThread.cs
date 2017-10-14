using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class FileMoverThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private const int MillisecondsTimeout = 5000;

        private readonly BlockingCollection<VideoProgressMetadata> _waitingForMoving;
        private readonly bool _overwrite;

        //TODO: overwrite config
        public FileMoverThread(string name, BlockingCollection<VideoProgressMetadata> waitingForMoving, bool overwrite = true) : base(name)
        {
            _waitingForMoving = waitingForMoving;
            _overwrite = overwrite;
        }

        public override void Start()
        {
            StopEvent = new ManualResetEvent(false);
            StoppedEvent = new ManualResetEvent(false);

            JobThread.Start();
            IsStarted = true;
        }

        public override void Stop()
        {
            StopEvent.Set();
        }

        protected override void Job()
        {
            //WaitOne equals 0 because we are waiting when we taking from queue, so we dont need to wait on stop event
            while (!StopEvent.WaitOne(0))
            {
                VideoProgressMetadata videoMetadata = null;
                try
                {
                    _waitingForMoving.TryTake(out videoMetadata, MillisecondsTimeout);
                    if (videoMetadata == null)
                    {
                        continue;
                    }
                    if (videoMetadata.MovingFilePath == null)
                    {
                        throw new Exception("MovingFilePath can't be null");
                    }
                    var sourceFilePath = videoMetadata.MovingFilePath;
                    var destFileName = FileHelper.RemoveIllegalPathCharacters(videoMetadata.Title) +
                                       Path.GetExtension(videoMetadata.MovingFilePath);
                    string destFilePath;
                    if (_overwrite)
                    {
                        destFilePath = Path.Combine(videoMetadata.SaveDir, destFileName);
                        if (File.Exists(destFilePath))
                        {
                            File.Delete(destFilePath);
                        }
                    }
                    else
                    {
                        destFilePath = FileHelper.GetAvailableFilePath(videoMetadata.SaveDir, destFileName);
                    }
                    Log.Info(LogHelper.Format($"Moving file... | source={sourceFilePath} | dest={destFilePath}", videoMetadata));
                    File.Move(sourceFilePath, destFilePath);
                    Log.Info(LogHelper.Format($"Moving file succeed", videoMetadata));
                    videoMetadata.Stage = VideoProgressStage.Completed;
                }
                catch (Exception ex)
                {
                    if (videoMetadata == null)
                    {
                        continue;
                    }
                    Log.Error(ex, LogHelper.Format($"Can't move file | saveDir={videoMetadata.SaveDir}", videoMetadata));
                    videoMetadata.Stage = VideoProgressStage.Error;
                    //TODO: Remove temp files
                }
            }
        }

        public override void Abort()
        {
            if (!IsAlive)
            {
                return;
            }
            Log.Warn($"Abort converter service ({Name})");
            JobThread.Abort();
        }
    }
}
