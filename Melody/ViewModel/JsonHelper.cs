using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.ViewModel
{
    public class JsonHelper
    {
        public static RootObject GetAllPlaylist()
        {
            
            StreamReader j = new StreamReader(@"Melody.json");
            var json = j.ReadToEnd();
            var output2 = JsonConvert.DeserializeObject<RootObject>(json);
            return output2;

        }
        public class Song
        {
            public string name { get; set; }
            public string genre { get; set; }
            public string artist { get; set; }
            public string audiofilepath { get; set; }
        }

        public class Playlist
        {
            public string name { get; set; }
            public List<Song> songs { get; set; }
            public string coverfilepath { get; set; }
        }

        public class RootObject
        {
            public List<Playlist> playlists { get; set; }
        }
    }
}
