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

            this.InitializeComponent();


            //MediaPlayerElement mediaPlayerElement1 = new MediaPlayerElement();
            //mediaPlayerElement1.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Media/video1.mp4"));
            //mediaPlayerElement1.AutoPlay = true;
        }

        private void PlayListSongSelectionEditView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Wait. Maybe we don't need to do anything when the selection
            // changes in the playlist editor list view.
            // I think we only need to do something about the state of the
            // selection when the user saves their playlist
        }

        private void PlayListSaveButton_Click(object sender, RoutedEventArgs e)
        {
            // What does sender refer to?
            // This never came up in Kal's example
            // e is going to refer to the button itself, I think
            // Not useful for deciding what songs to add to the new playlist

            // TODO: Can we ask the ListView which songs are selected at this point, or
            // do we have to keep track of which ones are selected/deselected as the
            // user is selecting songs?


            // TODO: Figure out how to get the default placeholder text
            // Provided by the control to be the content of the Text field
            // when it is queried by other controls
            var newPlaylist = new PlayList(PlayListName_UserInput.Text);
            // I think the answer is to set the Text property directly in Xaml
            // instead of using the PlaceholderText property

            foreach (var song in PlayListSongSelectionEditView.SelectedItems)
            {
                // We know that song is a Song because PlayListSongSelectionEditView
                // is bound to a List<Song>
                // Type cast song to Song so that it can be added to the list of
                // Song on newPlaylist
                newPlaylist.Songs.Add((Song)song);
            }

            // TODO: Have the ViewModel make this playlsit
            // give them the list of songs and the name that the user specified



            // DONE: Fix the fact that here we are adding the new playlist directly to the 
            // ObservableCollection<PlayList>
            // Only the ViewModel should write to this collection
            // In the Xamarin example, how does the View pass data to the
            // ViewModel?
            // Seems like the View (that is, right here in this event handler)
            // should call a method on the ViewModel class and passing it the
            // new playlist (then not keeping a reference to that new playlist
            // here in the View
            // ViewModel should be responsible for keeping track of the Playlist
            // from that point forward and (as relevant) populating it in the
            // ObservableCollection<PlayList> collection
            PlayListManager.AddNewPlayList(newPlaylist);

            // Update the View
            PlayListManager.GetAllPlayLists(displayingPlayLists);


            // TODO: Find out what the observer pattern says about
            // the View's ObservableCollection being updated directly by the ViewModel


            // ** QUESTION/IDEA: ?? Let the ViewModel keep a handle to the
            // View's ObservableCollection so that it can update it
            // when relevant...use case is for if playlists change
            // for some reason other than because of a UI event
            // A ViewModel's job is to be the bridge between the View and
            // the Model, that is each ViewModel is specialized to connect
            // specific View(s) with specific Model(s)
        }


        // When a playlist item is clicked
        private void PlayListMenuSidebarView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedPlayList = (PlayList)e.ClickedItem;
            // Populate the ObservableCollection<Song> displayingSongs with
            // the songs in the specified PlayList
            //PlayListManager.GetSongsByPlayList(displayingSongs, clickedPlayList.Name);


            PlayListManager.GetSongsByPlayList(displayingSongs, clickedPlayList);
            
            // View needs a list of the stuff that the view is supposed to display
            // The view is not supposed to generate that list
            // Even if it's trivial


            //10k songs in it
            // ui that is paginated
            // give me page 8 ofthis list
            // not doign that in the ui thread


            // another reason
            // dynamic playlist possible
            // all songs might be a dynamic playlist

            // songmanager has handle to all songs
            // new class that uses the 

            // it may not have a list of songs
            // when you ask me for a list of all the songs
            // I ask the songmanager for all the songs
            // go to sound manager

            // dynamic playlist random sort



            //displayingSongs.Clear();

            //foreach (var song in clickedPlayList.Songs)
            //{
            //    displayingSongs.Add(song);
            //}

            // Hide the playlist creator view
            PlayListCreationView.Visibility = Visibility.Collapsed;
            // Show the playlist playback view set to the clicked playlist
            PlayListPlayBackView.Visibility = Visibility.Visible;

            // If we ask the/a ViewModel to fill out the songs, we need to tell them
            // which songs somehow...that is, which playlist
            // Options include:
            // PlayList by its string Name (presuming that we don't allow duplicate names)
            // Reference to the PlayList directly...

            // MVVM probably wants us to communicate about the PlayList by Name
            // *sigh* is there a better way to do this and still be honoring
            // MVVM?

            // These aren't simply equivelant PlayList objects, they are
            // guaranteed to be exactly the same PlayList object, correct?
            // Makes best sense to...

            // Who's job should it be (View or ViewModel) to populate
            // ObservableCollection<Song> displayingSongs?
        }
    }
}
