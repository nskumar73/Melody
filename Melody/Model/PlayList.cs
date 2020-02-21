using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.Model
{
    //Playlist model to define playlist collection (e.g. All Songs)
    class PlayList
    {
        //Properties of playlist
        public string Name { get; set; }
        public string Cover { get; set; }
        //Playlistmodel type should be renamed to song
        public List<PlayListModel> Songs { get; set; }
    }
}
