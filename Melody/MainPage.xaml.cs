using Windows.System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Melody.Model;
using System.Collections.ObjectModel;
using System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Melody
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // DUMMY_PLAYLIST_COUNT guaranteed to be static
        // because it is const, correct?
        const int DUMMY_PLAYLIST_COUNT = 10;
        const int DUMMY_SONGS_COUNT = 8;

        
        private readonly ObservableCollection<PlayList> playLists;

        public MainPage()
        {
            this.InitializeComponent();

            playLists = new ObservableCollection<PlayList>();
            ConstructPlaceholderDataObjects();

        }


        public void ConstructPlaceholderDataObjects()
        {
            // Create the dummy All Songs playlist
            var AllSongs = new PlayList("All Songs");
            // Populate with dummy songs
            // (Represents the entire library of songs)
            for (var num = 1; num <= DUMMY_SONGS_COUNT; ++num)
            {
                AllSongs.Songs.Add(new Song
                {
                    Name = $"Placeholder Song Name {num}",
                    Artist = "Placeholder Song Artist",
                    Genre = "Placeholder Song Genre"
                });

            }
            // First PlayList in the collection will be All Songs
            playLists.Add(AllSongs);


            for (var num = 1; num <= DUMMY_PLAYLIST_COUNT; ++num)
            {
                playLists.Add(new PlayList($"Placeholder Playlist Name {num}"));
            }

            // Make some dummy playlists for the ObservableCollection
            //var playListCount = new int[DUMMY_PLAYLIST_COUNT];
            //foreach (var num in playListCount)
            //{
            // why is num == 0 on each loop iteration?
            //    playLists.Add(new PlayList($"Placeholder Playlist {num.ToString()}"));
            //}

            // Using Range was my first preference before using foreach
            // Why doesn't the new Range feature from C# 8 work? >_<
            // I was so excited, too
            // https://docs.microsoft.com/en-us/dotnet/api/system.range?view=netcore-3.1




        }
    }
}
