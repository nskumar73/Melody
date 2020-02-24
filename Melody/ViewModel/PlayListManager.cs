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
        // DUMMY_PLAYLIST_COUNT guaranteed to be static
        // because it is const, correct?
        const int DUMMY_PLAYLIST_COUNT = 5;
        const int DUMMY_SONGS_COUNT = 4;

        private static readonly PlayList allSongsPlayList = new PlayList("All Songs");
        private static readonly List<PlayList> allPlayLists = new List<PlayList>();
        // private static readonly List<Song> allSongs = new List<Song>();


        /// <summary>
        /// Fills out the caller's ObservableCollection<PlayList>with all playlists
        /// </summary>
        /// <param name="displayedPlayLists"></param>
        public static void GetAllPlayLists(
            ObservableCollection<PlayList> displayedPlayLists)
        {
            displayedPlayLists.Clear();
            foreach (var playList in allPlayLists)
            {
                displayedPlayLists.Add(playList);
            }
        }

        public static void GetSongsByPlayList(
            ObservableCollection<Song> ocSongs,
            PlayList playList)
        {
            // Find first match
            // Should be only match, as PlayList names are intended to be
            // unique
            // Maybe upgrading this to be a dictionary, kvp or something?
            //var playList = allPlayLists.First(pl => pl.Name == playListName);


            // string compares 

            // That's the M part of MVVM
            // When talk about MVC
            // Model thing on the heap
            // View reading that to do something

    
            ocSongs.Clear();

            foreach(var song in playList.Songs)
            {
                ocSongs.Add(song);
            }
        }

        public static void GetAllSongs(
            ObservableCollection<Song> ocSongs)
        {
            ocSongs.Clear();
            foreach (var song in allSongsPlayList.Songs)
            {
                ocSongs.Add(song); 
            }
        }


        // The All Songs playlist, beyond being the default playlist,
        // is also the list of songs that is offered to the user for
        // creating their own playlist.
        // Hrm. Honestly, it feels more appropriate and consistent to
        // do an ObservableCollection of Song for the playlist editing purposes
        // (mirrors the ObservableCollection of PlayList
        // Conceptually seperate the list/collection of all songs and the 
        // All Songs playlist, that is, the default playlist that 
        // happens to contain all the songs
        public static void GetAllSongsPlayList(ref PlayList displayedAllSongsPlayList)
        {
            displayedAllSongsPlayList = allSongsPlayList;
        }

        public static void AddNewPlayList(PlayList newPlayList)
        {
            allPlayLists.Add(newPlayList);

            // TODO: This should also update the ObservableCollection, right?
            // What causes this to happen?
            // The view needs to request it of the ViewModel, correct?
            // The ViewModel doesn't have a handle to the
            // ObservableCollection<PlayList>
        }



        public static void Setup()
        {
            // These calls temporally coupled in order for the
            // All Songs playlist to appear as the first playlist
            createDummyAllSongsPlayList();
            createSomeDummyPlayLists();
        }

        private static void createDummyAllSongsPlayList()
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

        private static void createSomeDummyPlayLists()
        {
            for (var num = 1; num <= DUMMY_PLAYLIST_COUNT; ++num)
            {
                allPlayLists.Add(new PlayList($"Placeholder Playlist Name {num}"));
            }

            // Make some dummy playlists for the ObservableCollection
            //var playListCount = new int[DUMMY_PLAYLIST_COUNT];
            //foreach (var num in playListCount)
            //{
            // why is num == 0 on each loop iteration?
            //    displayingPlayLists.Add(new PlayList($"Placeholder Playlist {num.ToString()}"));
            //}

            // Using Range was my first preference before using foreach
            // Why doesn't the new Range feature from C# 8 work? >_<
            // I was so excited, too
            // https://docs.microsoft.com/en-us/dotnet/api/system.range?view=netcore-3.1
        }
    }
}
