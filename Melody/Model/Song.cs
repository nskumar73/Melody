using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.Model
{
    /// <summary>
    /// This is model class for 
    /// </summary>
    public sealed class Song
    {
        /// <summary>
        /// Name of the song
        /// </summary>
       public string Name { get; set; }

        /// <summary>
        /// Genre of the song
        /// </summary>
        public string Genre { get; set; }
        

        /// <summary>
        /// Artist of the song
        /// </summary>
        public string Artist { get; set; }
    }
}
