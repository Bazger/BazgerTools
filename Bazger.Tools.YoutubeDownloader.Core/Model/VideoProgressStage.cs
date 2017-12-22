namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public enum VideoProgressStage
    {
        GettingPreview,
        Idling,
        Downloading,
        Exist,
        WaitingToConvertion, 
        Converting,
        Completed,
        Error,
        VideoUrlProblem,
        Moving
    }
}
