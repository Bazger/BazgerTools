﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NLog;

namespace Bazger.Tools.YouTubeDownloader.Core.Utility
{
    public static class YouTubeHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly string PlaylistVideosApiUrl = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet";

        public static string GetVideoId(string url)
        {
            IDictionary<string, string> query = HttpHelper.ParseQueryString(url);

            string v;
            return !query.TryGetValue("v", out v) ? null : v;
        }

        public static string GetPlaylistId(string url)
        {
            IDictionary<string, string> query = HttpHelper.ParseQueryString(url);

            string list;
            return !query.TryGetValue("list", out list) ? null : list;
        }

        public static IEnumerable<string> GetVideosUrls(string url, string youTubeApiKey)
        {
            if (GetPlaylistId(url) != null)
            {
                return GetPlaylistVideosUrls(url, youTubeApiKey);
            }
            if (GetVideoId(url) != null)
            {
                return new List<string>()
                {
                    url
                };
            }
            return null;
        }

        private static IEnumerable<string> GetPlaylistVideosUrls(string url, string youTubeApiKey)
        {
            string fullUrl = PlaylistVideosApiUrl +
                             $"&playlistId={GetPlaylistId(url)}&key={youTubeApiKey}&maxResults=50";
            string tokenUrl = string.Copy(fullUrl);
            var videoIds = new List<string>();
            do
            {
                var playlistJson = LoadPlaylistJson(tokenUrl);
                if (playlistJson == null)
                {
                    break;
                }
                videoIds.AddRange(GetVideosIdsFromPlaylist(playlistJson));
                Log.Info($"Received video urls: {videoIds.Count}");
                if (playlistJson["nextPageToken"] == null)
                {
                    break;
                }
                tokenUrl = fullUrl + $"&pageToken={playlistJson["nextPageToken"]}";

            } while (true);

            return videoIds.Select(videoId => "http://youtube.com/watch?v=" + videoId);
        }

        private static JObject LoadPlaylistJson(string url)
        {
            string playlistJson = HttpHelper.DownloadString(url);

            if (playlistJson == null)
            {
                return null;
            }

            return JObject.Parse(playlistJson);
        }

        private static IEnumerable<string> GetVideosIdsFromPlaylist(JObject json)
        {
            return json["items"].Select(videoInfo => videoInfo["snippet"]["resourceId"]["videoId"].ToString());
        }
    }
}
