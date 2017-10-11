namespace Bazger.Tools.YouTubeDownloader.Core.Converters
{
    public interface IAudioConverterProxy
    {
        string Convert(string path, string format);
        //For stoping correctly external converters 
        void Stop();
    }
}
