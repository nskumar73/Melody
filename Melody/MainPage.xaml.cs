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
            CreateNewPlayListHelper();

            //MediaPlayerElement mediaPlayerElement1 = new MediaPlayerElement();
            //mediaPlayerElement1.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Media/video1.mp4"));
            //mediaPlayerElement1.AutoPlay = true;
        }

        private enum ContentView
        {
            PlayListCreation,
            PlayListPlayBack,
            AddNewSong
        }

        // Manages the Visibility property on UI elements represented by
        // content views
        private void SwitchToContentView(ContentView destContentView)
        {
        
            switch (destContentView)
            {
                case ContentView.PlayListCreation:
                    PlayListPlayBackView.Visibility = Visibility.Collapsed;
                    PlayListCreationView.Visibility = Visibility.Visible;
                    // Hide the Add New Songs view
                    AddNewSongView.Visibility = Visibility.Collapsed;

                    break;
                case ContentView.PlayListPlayBack:
                    PlayListCreationView.Visibility = Visibility.Collapsed;
                    PlayListPlayBackView.Visibility = Visibility.Visible;
                    // Hide the Add New Songs view
                    AddNewSongView.Visibility = Visibility.Collapsed;

                    break;
                case ContentView.AddNewSong:
                    // Hide the playlist creator view
                    PlayListCreationView.Visibility = Visibility.Collapsed;
                    // Hide the playlist playback view
                    PlayListPlayBackView.Visibility = Visibility.Collapsed;
                    // Show the Add New Songs view
                    AddNewSongView.Visibility = Visibility.Visible;
                    break;
            }
        }


        private void PlayListSaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Share playlist save logic with pressing Enter from the
            // user input TextBox
            SaveNewPlayListHelper();
        }

        private void SelectPlayListInMenuSidebarView(PlayList playList)
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
            SwitchToContentView(ContentView.PlayListPlayBack);
        }

        // When the user clicks new playlist button
        private void CreateNewPlayListButton_Click(
                                        object sender, RoutedEventArgs e)
        {
            CreateNewPlayListHelper();
        }



        const string PLAYLIST_NAME_TEMPLATE = "New Playlist ";

        // If input matches playlist name template, return the number of the
        // template, otherwise return null

        // Good article on nullable value types
        // https://www.tutorialsteacher.com/csharp/csharp-nullable-types
        // Note: "int?" is equivelant to "Nullable<int>"
        private static int? GetTemplateMatchNum(string nameToCompare, string template)
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
        private void CreateNewPlayListHelper()
        {
            // Right now we're figuring out what default playlist name to offer to the user
            // Find the default playlist name with the biggest number N
            // Offer a name ending with the next number (N + 1)
            int? largestPlayListNumMatch = null;
            foreach (var playList in displayingPlayLists)
            {
                // If the playlist name matches the template text
                // and its number is the new largest match
                int? templateMatchNum = GetTemplateMatchNum(playList.Name, PLAYLIST_NAME_TEMPLATE);
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


            var playListNameNumToOffer = (largestPlayListNumMatch.HasValue ? largestPlayListNumMatch.Value + 1 : 1);

            StringBuilder playListNameToOfferSB = new StringBuilder(PLAYLIST_NAME_TEMPLATE);
            playListNameToOfferSB.Append(playListNameNumToOffer);




            // Write the default playlist offer into the user input text box

            PlayListName_UserInput.Text =
                                playListNameToOfferSB.ToString();

            // Display all songs
            PlayListManager.GetAllSongs(displayingSongs);

            // Finally, show the PlayListCreationView
            SwitchToContentView(ContentView.PlayListCreation);
        }

        private void PlayListName_UserInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                // Share playlist save logic with the "Save Playlist" button
                SaveNewPlayListHelper();
            }
        }

        private void SaveNewPlayListHelper()
        {
            // TODO: Figure out why ShowMode is not working with Flyout, that is,
            // it is not showing up as a member of Flyout for some reason
            //((Button)sender).Flyout.ShowMode = FlyoutShowMode.TransientWithDismissOnPointerMoveAway;

            // If the user has neglected to select any songs for their playlist
            if (PlayListSongSelectionEditView.SelectedItems.Count == 0)
            {
                // Show a flyout explaining why the playlist can't be saved
                FlyoutText_for_PlayListSaveButton.Text =
                    "Choose some songs first.";
                FlyoutBase.ShowAttachedFlyout(PlayListSaveButton);
                // See: "How to create a flyout"
                // https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/dialogs-and-flyouts/flyouts#how-to-create-a-flyout

                // ABORT saving the playlist
                // (Remain in PlayListCreationView, retaining the user input or
                // default offered playlist name)
                return;
            }



            // Ignore leading and trailing whtiespace from user input
            // Note that this is not just for comparison;
            // disallow leading and trailing whitespace from playlist names
            // because that is awkward, confusing and probably unintentional
            string newPlayListNameUserInput = PlayListName_UserInput.Text.Trim();


            // If user deleted the default playlist offer text, leaving the input empty
            if (newPlayListNameUserInput.Length == 0)
            {
                // Show a flyout explaining why the playlist can't be saved
                FlyoutText_for_PlayListName_UserInput.Text =
                    "Please set a name for your playlist.";
                FlyoutBase.ShowAttachedFlyout(PlayListName_UserInput);

                // ABORT saving the playlist
                // (Remain in PlayListCreationView, retaining any user-selected songs)
                return;
            }


            // Get a handle to the observable collection that we can use with a LINQ query
            bool playListNameAlreadyUsed = displayingPlayLists.ToList().Exists(
                playList => String.Equals(playList.Name, newPlayListNameUserInput));

            if (playListNameAlreadyUsed)
            {
                // Show a flyout explaining why the playlist can't be saved
                FlyoutText_for_PlayListName_UserInput.Text =
                    "Playlist name is already taken. Please use a new name.";
                FlyoutBase.ShowAttachedFlyout(PlayListName_UserInput);

                // ABORT saving the playlist
                // (Remain in PlayListCreationView, retaining any user-selected songs)
                return;
            }

            

            //IEnumerable<Type> symbol = TheCollection.Cast<Type>();
            // Optionally, if the IEnumberable<Type> doesn't do it for you:
            //var list = new List<Type>(symbol);
            // Converting observable collection back to regular collection
            // https://stackoverflow.com/a/1658656

            

            // Create an IEnumerable<Song> that we can pass to the PlayListManager
            // so that it can iterate through the list of selected songs 
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
                PlayListManager.CreateNewPlayList(newPlayListNameUserInput, selectedSongs);

            // Re-populate the view's playlist collection
            // to reflect the new playlist
            PlayListManager.GetAllPlayLists(displayingPlayLists);

            // Prepare the view's songs collection to be the songs from the
            // new playlist
            PlayListManager.GetSongsByPlayList(displayingSongs, newPlaylist);

            // Switch to the songs view
            SwitchToContentView(ContentView.PlayListPlayBack);

            // Select/highlight the new playlist in the left sidebar view
            SelectPlayListInMenuSidebarView(newPlaylist);
        }



        private void PlayListSongSelectionEditView_SelectionChanged(
                                    object sender, SelectionChangedEventArgs e)
        {
            // TODO: Figure out the correct way to delete this event
            //       handler without getting compiler errors in generated code.
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

            SwitchToContentView(ContentView.AddNewSong);
        }

        
        //Andrea
        //Hello! So above is the button to open the file dialogue, and you'll see i have a songpath save there.. just park that thought for now
        //Below is the song save button that's supposed to save the song and add to the collection after the user adds the song name, title, etc.
        //For the first part, my thought was to convert all the input text into the parameters needed to create the song
        //Im gonna send you a screenshot on slack real quick
        //kk, i sent you a picture on slack on how i approached the songs portion, starting with converting the input text

        // k
        private void SongSaveButton_Click(object sender, RoutedEventArgs e)
        {

            //So the text inputs below can stay as part of the View, correct?
            //The next step would be to create a method in the playlistmanager that the view can call below?
            //okay!

            // let's see yes here below you are setting up to make a song
            // so all you'd need to do is make the CreateNewSong in playlist manaager like you said
            // and pass these in
            // then PlayListManager can add it on the back end

            // one moment, let me peek at the summary of what I was I guess thinkign you were up to ...
            // pasted a screenshot in slack...let's see if this mirrors what you are thinking/what you've done




            //Allow user to save name, artist, and genre
            string name = SongTitle_UserInput.Text;
            string artist = SongArtist_UserInput.Text;
            string genre = Genre_UserInput.Text;
            string filepath = "test";

            //TODO Copy over filepath

            //Creates new song to add to all songs playlist
            var newSong = PlayListManager.AddNewSong(name, artist, genre);

            PlayListManager.GetAllSongs(displayingSongs);

            SwitchToContentView(ContentView.PlayListPlayBack);
        }
    }
}
