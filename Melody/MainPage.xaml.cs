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
        // PLAYLIST_COUNT guaranteed to be static
        // because it is const, correct?
        const int PLAYLIST_COUNT = 10;
        
        private readonly ObservableCollection<PlayList> playLists;

        public MainPage()
        {
            this.InitializeComponent();

            playLists = new ObservableCollection<PlayList>();
            for (var num = 1; num <= PLAYLIST_COUNT; ++num)
            {
                playLists.Add(new PlayList($"Placeholder Playlist {num}"));           
            }

            // Make some dummy playlists for the ObservableCollection
            //var playListCount = new int[PLAYLIST_COUNT];
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
