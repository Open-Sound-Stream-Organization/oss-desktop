using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Artist
    {
        public Artist(string Name)
        {
            name = Name ?? throw new ArgumentNullException(nameof(Name));
            Albums = new List<Album>();

            Artists.Add(this);
        }

        public static List<Artist> Artists { get; set; }

        public List<Album> Albums { get; set; }

        public int? id { get; set; }

        public string name { get; set; }
    }
}