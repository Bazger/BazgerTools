using System.Configuration;
using System.IO;
using System.Net.Http;
using Bazger.Tools.YouTubeDownloader.Core.Model;
using Bazger.Tools.YouTubeDownloader.Core.Utility;
using Jint;
using Newtonsoft.Json.Linq;

namespace Bazger.Tools.YouTubeDownloader.Core.WebSites
{
    public class YouTubeMp3Proxy : IWebSiteDownloaderProxy
    {
        private const string WebSiteUrl = @"http://www.youtube-mp3.org";
        private static readonly string SessionKeyJsScript = File.ReadAllText(ConfigurationManager.AppSettings["SessionKeyJsScript"]);

        public void Download(VideoProgressMetadata videoMetadata)
        {
            string videoId = YouTubeHelper.GetVideoId(videoMetadata.Url);
            string videoInfo = GetVideoInfoJson(videoId);
            if (videoInfo == null)
            {
                throw new HttpRequestException("Can't get json | video url: " + videoMetadata.Url);
            }

            dynamic videoInfoJson = JObject.Parse(videoInfo);
            string downloadUrl = GenerateDownloadUrl(videoId, videoInfoJson);

            Stream stream = HttpHelper.DownloadStream(downloadUrl);

            string videoName = videoInfoJson.title.ToString();
            string videoPath = Path.Combine(videoMetadata.OutputDirectory, videoName + ".mp3");
            videoMetadata.VideoFilePath = videoPath;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                using (var file = new FileStream(videoPath, FileMode.Create, FileAccess.Write))
                {
                    ms.WriteTo(file);
                }
            }
        }

        private static string GenerateDownloadUrl(string videoId, dynamic json)
        {
            string url =
                WebSiteUrl + $"/get?video_id={videoId}&ts_create={json.ts_create.ToString()}&r={json.r.ToString()}&h2={json.h2}";
            url += $"&s={GenerateSessionKey(url)}";
            return url;
        }

        private static string ClearVideoInfoJson(string json)
        {
            int startBacket = json.IndexOf("{");
            int endBracket = json.LastIndexOf("}");
            return json.Substring(startBacket, endBracket - startBacket + 1);
        }

        private static string GetVideoInfoJson(string videoId)
        {
            string url = WebSiteUrl + $"/a/itemInfo/?video_id={videoId}&ac=www&t=grp&r=1493473525901&s=20040";
            return ClearVideoInfoJson(HttpHelper.DownloadString(url));
        }

        private static string GenerateSessionKey(string value)
        {
            Engine engine = new Engine();
            engine.SetValue("H", value);
            engine.Execute(SessionKeyJsScript);
            return engine.GetValue("N").ToString();
        }
    }
}