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
using System.Text;
using Windows.Storage;

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

            InitializeComponent();
            displayingPlayLists = new ObservableCollection<PlayList>();
            displayingSongs = new ObservableCollection<Song>();

            PlayListManager.Setup();

            PlayListManager.GetAllPlayLists(displayingPlayLists);

            // Immediately enter playlist creation
            createNewPlayListHelper();



            //MediaPlayerElement mediaPlayerElement1 = new MediaPlayerElement();
            //mediaPlayerElement1.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Media/video1.mp4"));
            //mediaPlayerElement1.AutoPlay = true;
        }

        private enum contentView
        {
            PlayListCreation,
            PlayListPlayBack,
            AddNewSong
        }

        // Manages the Visibility property on UI elements represented by
        // content views
        private void switchToContentView(contentView contentView)
        {
            switch (contentView)
            {
                case contentView.PlayListCreation:
                    // Hide the playlist playback view
                    PlayListPlayBackView.Visibility = Visibility.Collapsed;
                    // Show the playlist creator view
                    PlayListCreationView.Visibility = Visibility.Visible;
                    // Hide the Add New Songs view
                    AddNewSongView.Visibility = Visibility.Collapsed;

                    break;
                case contentView.PlayListPlayBack:
                    // Hide the playlist creator view
                    PlayListCreationView.Visibility = Visibility.Collapsed;
                    // Show the playlist playback view set to the clicked playlist
                    PlayListPlayBackView.Visibility = Visibility.Visible;
                    // Hide the Add New Songs view
                    AddNewSongView.Visibility = Visibility.Collapsed;

                    break;
                case contentView.AddNewSong:
                    // Hide the playlist creator view
                    PlayListCreationView.Visibility = Visibility.Collapsed;
                    // Hide the playlist playback view
                    PlayListPlayBackView.Visibility = Visibility.Collapsed;
                    // Show the Add New Songs view
                    AddNewSongView.Visibility = Visibility.Visible;
                    break;
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
            // TODO: Figure out why ShowMode is not working with Flyout, that is,
            // it is not showing up as a member of Flyout for some reason
            //((Button)sender).Flyout.ShowMode = FlyoutShowMode.TransientWithDismissOnPointerMoveAway;

            // If the user has neglected to select any songs for their playlist
            if (PlayListSongSelectionEditView.SelectedItems.Count == 0)
            {
                // Show the flyout explaining why they can't save the playlist
                FlyoutBase.ShowAttachedFlyout(PlayListSaveButton);
                // See: "How to create a flyout"
                // https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/dialogs-and-flyouts/flyouts#how-to-create-a-flyout

                // ABORT saving the playlist
                return;
            }


            string newPlayListName = PlayListName_UserInput.Text;

            // ** TODO: Figure out how to get the default placeholder text
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
            createNewPlayListHelper();
        }


        //// Must ensure that the playlist num offered is unique
        //// that is, it is not represented in the existing playlists
        //// Check the list of playlists to see if there are any that match
        //// the default template (whether they were generated, loaded from file
        //// or the user was funny and typed it)
        //// If yes, find the highest one and start there
        //// OR simply skip the taken ones?
        //// Do the scan when the starting playlists are first loaded
        //// then handle each new playlist as it is created
        //// Find next candidate, then check the list for it
        ////
        //private static class playListNumOfferManager
        //{
        //    // App always starts with a default playlist offer of 1
        //    private static int currentOffer = 1;

        //    // Using "Peek" method instead of a property with a getter
        //    // to emphasize that the usual way of accessing the current
        //    // offer is to collect it as it is created
        //    public static int PeekCurrentOffer()
        //    {
        //        return currentOffer;
        //    }

        //    public static int GetNewOffer()
        //    {
        //        return ++currentOffer;

        //    }

        //    public static int DirectSetNextOffer(int nextOffer)
        //    { 
            
        //    }
        //}


        static readonly string PLAYLIST_NAME_TEMPLATE = "New Playlist ";

        // If input matches playlist name template, return the number of the
        // template, otherwise return null

        // Good article on nullable value types
        // https://www.tutorialsteacher.com/csharp/csharp-nullable-types
        // Note: "int?" is equivelant to "Nullable<int>"
        private static int? getTemplateMatchNum(string nameToCompare, string template)
        {
            // If the name to compare contains exactly one instance of the template
            var index = nameToCompare.IndexOf(template);
            if (index >= 0 && index == nameToCompare.LastIndexOf(template))
            {
                // Drop the matching template text
                string parsingString = nameToCompare.Remove(index, template.Length);

                // Ignores leading and trailing whitespace by design
                if (int.TryParse(parsingString, out int parsedNumber))
                {
                    return parsedNumber;
                }
            }
            return null;
        }


        // When the app first loads and also
        // When the user clicks the new playlist button
        private void createNewPlayListHelper()
        {
            // Display all songs
            PlayListManager.GetAllSongs(displayingSongs);


            // Do this once when the app loads
            // For each playlist in all of the playlists
            // Find matches to the default pattern
            // If find a match, note its number
            // Keep track of what the highest number is
            // That's where we will pick up with the offers

            // Seems like the View should ask the ViewModel to do this
            // Doesn't make senese for the view to do this with the
            // ObservableCollection<PlayList>, right?

            // Well, for now:



            // Right now we're figuring out what default playlist name to offer to the user
            // Find the default playlist name with the biggest number N
            // Offer a name ending with the next number (N + 1)
            int? largestPlayListNumMatch = null;
            foreach (var playList in displayingPlayLists)
            {
                // If the playlist name matches the template text
                // and its number is the new largest match
                int? templateMatchNum = getTemplateMatchNum(playList.Name, PLAYLIST_NAME_TEMPLATE);
                if (templateMatchNum.HasValue && templateMatchNum.Value > largestPlayListNumMatch.GetValueOrDefault())
                                                                            // 0 is the default for int if int? is null
                {
                    // Store the new largest match
                    largestPlayListNumMatch = templateMatchNum.Value;
                }
            }
            // Now either largestPlayListNumMatch is the number of the largest match or it is still null

            // If null, that means that there were no matches and we offer the starting default of 1
            // If not null, (rather, if N), then we offer N+1






            // ***** TODO: More elegantly handle default offered playlist name
            // See also: PlayListName_UserInput Text Box in xaml
            var playListNameNumToOffer = (largestPlayListNumMatch.HasValue ? largestPlayListNumMatch.Value + 1 : 1);

            StringBuilder playListNameToOfferSB = new StringBuilder(PLAYLIST_NAME_TEMPLATE);
            playListNameToOfferSB.Append(playListNameNumToOffer);

            PlayListName_UserInput.Text = playListNameToOfferSB.ToString();



            // Now, before refactoring, does it work?



            switchToContentView(contentView.PlayListCreation);
        }

        private void PlayListMenuSidebarView_SelectionChanged(
                                    object sender, SelectionChangedEventArgs e)
        {
            // TODO: Figure out the correct way to delete this event
            //               handler (PlayListMenuSidebarView_SelectionChanged) without
            //               getting compiler errors in generated code.

        }

        
        
        private async void AddNewSongButton_Click(object sender, RoutedEventArgs e)
        {
            //Instantiates File Open picker and opens the dialogue
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;

            //Filters the type of files acceptable to connect
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".mp4");
            
            //Allows user to select the song
            StorageFile song = await picker.PickSingleFileAsync();
            if (song != null)
            {
                var stream = await song.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var songpath = song.Path;
                //await song.CopyAsync(musicFolder, song.Name, NameCollisionOption.GenerateUniqueName);

            }

            switchToContentView(contentView.AddNewSong);
        }

        
        
        private void SongSaveButton_Click(object sender, RoutedEventArgs e)
        {

            //Allow user to save name, artist, and genre
            string name = SongTitle_UserInput.Text;
            string artist = SongArtist_UserInput.Text;
            string genre = Genre_UserInput.Text;
            string filepath = "test";

            //Copy over the audiofilepath
            //string filepath = 

            //Create new song 
            Song addsong = new Song(name, artist, genre);


            //add song to observable collection
            
            

            //allSongsPlayList.Songs.Add(new Song
            //    {
            //        Name = $"Placeholder Song Name {num}",
            //        Artist = "Placeholder Song Artist",
            //        Genre = "Placeholder Song Genre"
            //    })

            switchToContentView(contentView.PlayListPlayBack);
        }
    }
}
