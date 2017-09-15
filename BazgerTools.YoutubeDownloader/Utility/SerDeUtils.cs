using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BazgerTools.YouTubeDownloader.Utility
{
    public static class SerDeUtils
    {
        public static TObject DeserializeJsonFile<TObject>(string path) where TObject : new()
        {
            return JsonConvert.DeserializeObject<TObject>(File.ReadAllText(path));
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
    }
}