using Melody.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.ViewModel
{
    public static class JsonHelper
    {
        public static RootObject GetAllPlaylist()
        {

            StreamReader j = new StreamReader(@"Assets\Melody.json");
            var json = j.ReadToEnd();
            var output2 = JsonConvert.DeserializeObject<RootObject>(json);
            return output2;

        }


        public static void WritePlayList(RootObject output)
        {
            using (StreamWriter file = File.CreateText(@"Assets\Melody.json"))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, output);

            }
        }


        public class RootObject
        {
            public List<PlayList> playlists { get; set; }
        }
    }
}
