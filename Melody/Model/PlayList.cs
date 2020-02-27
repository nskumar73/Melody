using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.Model
{
    /// <summary>
    /// Model for a playlist that the user can add songs to and select a cover for
    /// </summary>
    public sealed class PlayList // : INotifyPropertyChanged (see TODO)
    {
        /// <summary>
        /// Name of the playlist
        /// Example: "Disco Hits"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path to the image file of the playlist
        /// Example: "\Assets\Images\Disco Ball.png"
        /// </summary>
        public string CoverFilePath { get; set; }

        /// <summary>
        /// List of songs in the playlist
        /// </summary>
        public readonly List<Song> Songs;
        // TODO: Protect the songs list from arbitrary editing by users of the 
        // PlayList class

        public PlayList(string name)
        {
            Name = name;
            CoverFilePath = "/Assets/PlaylistCoverPlaceholder.png";
            Songs = new List<Song>();
        }

        // TODO: PlayList (or maybe a wrapper around it?)
        // must implement INotifyPropertyChanged in order for the UI to update
        // when PlayList objects inside it change state
        // https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=netframework-4.8
    }
}
