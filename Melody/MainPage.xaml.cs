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
using Melody.ViewModel;
using Windows.Media.Core;

namespace Melody
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {   
        private readonly ObservableCollection<PlayList> displayingPlayLists;
        private readonly ObservableCollection<Song> displayingSongs;

        // Which playlist is represented by these songs is not explicitly stored
        // Rather, handled entirely in the PlayListMenuSidebarView_ItemClick event handler

        public MainPage()
        {
            displayingPlayLists = new ObservableCollection<PlayList>();

            //!! Recall that the Songs property on a PlayList is a List<Song>, 
            // not an ObservableCollection<Song>
            displayingSongs = new ObservableCollection<Song>();
                        
            PlayListManager.Setup();

            PlayListManager.GetAllPlayLists(displayingPlayLists);
            PlayListManager.GetAllSongs(displayingSongs);

            InitializeComponent();

            //MediaPlayerElement mediaPlayerElement1 = new MediaPlayerElement();
            //mediaPlayerElement1.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Media/video1.mp4"));
            //mediaPlayerElement1.AutoPlay = true;
        }


        private enum contentView
        {
            PlayListCreation,
            PlayListPlayBack
            // TODO: Add option for the import-song-to-library view
        }

        private void switchToContentView(contentView contentView)
        {
            switch (contentView)
            {
                case contentView.PlayListCreation:
                    // Hide the playlist playback view
                    PlayListPlayBackView.Visibility = Visibility.Collapsed;
                    // Show the playlist creator view
                    PlayListCreationView.Visibility = Visibility.Visible;

                    break;
                case contentView.PlayListPlayBack:
                    // Hide the playlist creator view
                    PlayListCreationView.Visibility = Visibility.Collapsed;
                    // Show the playlist playback view set to the clicked playlist
                    PlayListPlayBackView.Visibility = Visibility.Visible;

                    break;
                    // TODO: Add option for the import-song-to-library view
            }
        }

        private void PlayListSongSelectionEditView_SelectionChanged(
                                    object sender, SelectionChangedEventArgs e)
        {
            // TODO: Figure out the correct way to delete this event
            //       handler without getting compiler errors in generated code.
        }

        private void PlayListSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newPlayListName = PlayListName_UserInput.Text;

            // TODO: Figure out how to get the default placeholder text
            // Provided by the control to be the content of the Text field
            // when it is queried by other controls

            // I think the answer is to set the Text property directly in Xaml
            // instead of using the PlaceholderText property



            
            // Create an IEnumerable<Song> that we can pass to the PlayListManager
            // so that it can iterate through the list of selected songs without
            // needing to know about 
            IEnumerable<Song> selectedSongs =
                PlayListSongSelectionEditView.SelectedItems.Cast<Song>();
            // Following this example for how to use the Cast method on IEnumerable:
            // https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.cast?view=netframework-4.8
            // See also the ListViewBase.SelectedItems property:
            // https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.listviewbase.selecteditems


            // Pass the user input about the new playlist to the ViewModel
            // so the PlayList object to be created and stored/managed on the
            // back end
            // Store a reference to the new PlayList, we'll use it to
            // populate the playlist's songs into the view (avoiding doing
            // a lookup by playlist string name)
            var newPlaylist =
                PlayListManager.CreateNewPlayList(newPlayListName, selectedSongs);

            // Re-populate the view's playlist collection
            // to reflect the new playlist
            PlayListManager.GetAllPlayLists(displayingPlayLists);

            // Prepare the view's songs collection to be the songs from the
            // new playlist
            PlayListManager.GetSongsByPlayList(displayingSongs, newPlaylist);

            // Switch to the songs view
            switchToContentView(contentView.PlayListPlayBack);

            // Select/highlight the new playlist in the left sidebar view
            selectPlayListInMenuSidebarView(newPlaylist);
        }

        private void selectPlayListInMenuSidebarView(PlayList playList)
        {
            // Look in the PlayListMenuSidebarView for the ListViewItem
            // that corresponds to the playList
            var playListItem = PlayListMenuSidebarView.Items.ToList().First(
                                     item => item == playList);
            // Note that playListItem is of type ListViewItem:
            // https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.ListViewItem

            // Set the current selection of PlayListMenuSidebarView to correspond to the
            // PlayList that corresponds to playListItem
            PlayListMenuSidebarView.SelectedItem = playListItem;
        }

        // When a playlist item is clicked
        private void PlayListMenuSidebarView_ItemClick(
                                        object sender, ItemClickEventArgs e)
        {
            var clickedPlayList = (PlayList)e.ClickedItem;

            // Populate the ObservableCollection<Song> displayingSongs with
            // the songs in the specified PlayList
            PlayListManager.GetSongsByPlayList(displayingSongs, clickedPlayList);

            // Show the playlist playback view set to the clicked playlist
            switchToContentView(contentView.PlayListPlayBack);
        }

        // When the user clicks new playlist button
        private void CreateNewPlayListButton_Click(
                                        object sender, RoutedEventArgs e)
        {
            // Load all songs
            PlayListManager.GetAllSongs(displayingSongs);

            // Clear the playlist name text entry box

            // TODO: More elegantly handle default offered playlist name
            // See also: PlayListName_UserInput Text Box in xaml
            PlayListName_UserInput.Text = "New Playlist N";

            switchToContentView(contentView.PlayListCreation);
        }

        private void PlayListMenuSidebarView_SelectionChanged(
                                    object sender, SelectionChangedEventArgs e)
        {
            // TODO: Figure out the correct way to delete this event
            //               handler (PlayListMenuSidebarView_SelectionChanged) without
            //               getting compiler errors in generated code.

        }
    }
}
