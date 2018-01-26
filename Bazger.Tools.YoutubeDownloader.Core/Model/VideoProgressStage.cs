namespace Bazger.Tools.YouTubeDownloader.Core.Model
{
    public enum VideoProgressStage
    {
        Idling,
        GettingPreview,
        PreviewFound,
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
