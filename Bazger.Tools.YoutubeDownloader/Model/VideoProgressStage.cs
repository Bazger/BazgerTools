namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public enum VideoProgressStage
    {
        Downloading,
        Exist,
        WaitingToConvertion, 
        Converting,
        Completed,
        Error,
        VideoUrlProblem
    }
}
