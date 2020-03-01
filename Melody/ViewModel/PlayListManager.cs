using Melody.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.ViewModel
{
    // Right now PlayListManager is managing both playlists and songs
    // QUESTION: Is it appropriate for there to be another ViewModel class
    // called SongManager?
    public static class PlayListManager
    {
        // Guaranteed to be static because it is const, correct?
        private const int DUMMY_SONGS_COUNT = 4;

        private static readonly PlayList allSongsPlayList = new PlayList("All Songs");
        private static readonly List<PlayList> allPlayLists = new List<PlayList>();
        // private static readonly List<Song> allSongs = new List<Song>();


        /// <summary>
        /// Fills out the caller's ObservableCollection<PlayList> with all playlists
        /// </summary>
        /// <param name="displayedPlayLists"></param>
        public static void GetAllPlayLists(
                                    ObservableCollection<PlayList> ocPlayLists)
        {
            // Observer pattern specifies to clear the ObservableCollection 
            ocPlayLists.Clear();

            foreach (var playList in allPlayLists)
            {
                ocPlayLists.Add(playList);
            }
        }

        public static void GetSongsByPlayList(
                                        ObservableCollection<Song> ocSongs,
                                        PlayList playList)
        {
            // Observer pattern specifies to clear the ObservableCollection 
            ocSongs.Clear();

            foreach(var song in playList.Songs)
            {
                ocSongs.Add(song);
            }
        }

        public static void GetAllSongs(ObservableCollection<Song> ocSongs)
        {
            // Observer pattern specifies to clear the ObservableCollection 
            ocSongs.Clear();
            foreach (var song in allSongsPlayList.Songs)
            {
                ocSongs.Add(song); 
            }
        }

        public static PlayList CreateNewPlayList(
                                        string name, IEnumerable<Song> songs)
        {
            var newPlaylist = new PlayList(name);
            songs.ToList().ForEach( song => newPlaylist.Songs.Add(song) );
            allPlayLists.Add(newPlaylist);
            return newPlaylist;
        }

        public static void Setup()
        {
            CreateDummyAllSongsPlayList();
            //JSONHelper.GetAllPlayLists(allPlayLists);
        }

        private static void CreateDummyAllSongsPlayList()
        {
            // Populate with dummy songs
            // (Represents the entire library of songs)
            for (var num = 1; num <= DUMMY_SONGS_COUNT; ++num)
            {
                allSongsPlayList.Songs.Add(new Song
                {
                    Name = $"Placeholder Song Name {num}",
                    Artist = "Placeholder Song Artist",
                    Genre = "Placeholder Song Genre"
                });
            }

            // First PlayList in the collection will be All Songs
            // Or not? Maybe need to special-case this?
            allPlayLists.Add(allSongsPlayList);
        }
    }
}
