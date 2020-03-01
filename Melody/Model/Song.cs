using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.Model
{
    /// <summary>
    /// Model for a song that the user can add to a playlist.
    /// </summary>
    public sealed class Song
    {
        /// <summary>
        /// Name of the song
        /// Example: "I Will Survive"
        /// </summary>
       public string Name { get; set; }

        /// <summary>
        /// Genre of the song
        /// Example: "Disco"
        /// </summary>
        public string Genre { get; set; }
        

        /// <summary>
        /// Artist of the song
        /// Example: "Gloria Gaynor"
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Path to the audio file of the song relative to the folder of the application
        /// Example: "\Assets\Songs\I Will Survive - Gloria Gaynor.mp3"
        /// </summary>
        public string AudioFilePath { get; set; }

        public Song (string name, string artist, string genre)
        {
            Name = name;
            Artist = artist;
            Genre = genre;
            
        }
    }
}
