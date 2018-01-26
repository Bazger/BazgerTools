using System;
using System.Collections.Concurrent;
using System.IO;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public class FileMoverThread : ServiceThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private const int MillisecondsTimeout = 1000;

        private readonly BlockingCollection<VideoProgressMetadata> _waitingForMoving;
        private readonly bool _overwrite;

        public FileMoverThread(string name, BlockingCollection<VideoProgressMetadata> waitingForMoving, bool overwrite = true) : base(name)
        {
            _waitingForMoving = waitingForMoving;
            _overwrite = overwrite;
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
                    var destFilePath = Path.Combine(videoMetadata.SaveDir, destFileName);
                    if (_overwrite)
                    {

                        if (File.Exists(destFilePath))
                        {
                            File.Delete(destFilePath);
                        }
                    }
                    else
                    {
                        destFilePath = FileHelper.GetAvailableFilePath(destFilePath);
                    }
                    Log.Info(LogHelper.Format($"Moving file... | source={sourceFilePath} | dest={destFilePath}", videoMetadata));
                    File.Move(sourceFilePath, destFilePath);
                    Log.Info(LogHelper.Format($"Moving file succeed", videoMetadata));
                    videoMetadata.Stage = VideoProgressStage.Completed;
                    videoMetadata.VideoFilePath = destFilePath;
                }
                catch (Exception ex)
                {
                    if (videoMetadata == null)
                    {
                        continue;
                    }
                    Log.Error(ex, LogHelper.Format($"Can't move file | saveDir={videoMetadata.SaveDir}", videoMetadata));
                    videoMetadata.Stage = VideoProgressStage.Error;
                    videoMetadata.ErrorArgs = ex.ToString();
                    try
                    {
                        if (File.Exists(videoMetadata.DownloadedVideoFilePath))
                        {
                            File.Delete(videoMetadata.DownloadedVideoFilePath);
                        }
                    }
                    catch (Exception innerEx)
                    {
                        Log.Warn(innerEx, LogHelper.Format($"Can't delete video file | path={videoMetadata.DownloadedVideoFilePath}", videoMetadata));
                    }
                    try
                    {
                        if (File.Exists(videoMetadata.ConvertedFilePath))
                        {
                            File.Delete(videoMetadata.ConvertedFilePath);
                        }
                    }
                    catch (Exception innerEx)
                    {
                        Log.Warn(innerEx, LogHelper.Format($"Can't delete converted audio file | path={videoMetadata.ConvertedFilePath}", videoMetadata));
                    }
                }
                base.Job();
            }
        }
    }
}
