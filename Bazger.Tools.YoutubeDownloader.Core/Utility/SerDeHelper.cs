using System.IO;
using Newtonsoft.Json;

namespace Bazger.Tools.YouTubeDownloader.Core.Utility
{
    public static class SerDeHelper
    {
        public static TObject DeserializeJsonFile<TObject>(string path) where TObject : new()
        {
            return JsonConvert.DeserializeObject<TObject>(File.ReadAllText(path));
        }

        public static TObject DeserializeJsonFile<TObject>(string path, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<TObject>(File.ReadAllText(path), settings);
        }

        public static void SerializeToJsonFile<TObject>(TObject results, string path)
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(path))
            {
                using (JsonWriter writer = new JsonTextWriter(sw) { Formatting = Formatting.Indented })
                {
                    serializer.Serialize(writer, results);
                }
            }            
        }

        public static void SerializeToJsonFile(object results, string path, JsonSerializerSettings settings, Formatting formatting = Formatting.Indented)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(results, formatting, settings));
        }
    }
}