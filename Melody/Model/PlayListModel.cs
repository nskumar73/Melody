using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.Model
{
    /// <summary>
    /// This is model class for Play list
    /// </summary>
    public sealed class PlayListModel
    {
        /// <summary>
        /// Name of the artist
        /// </summary>
       public string Name { get; set; }

        /// <summary>
        /// Genre of the music
        /// </summary>
        public string Genre { get; set; }
        

        /// <summary>
        /// Artist Name
        /// </summary>
        public string Artist { get; set; }
    }
}
