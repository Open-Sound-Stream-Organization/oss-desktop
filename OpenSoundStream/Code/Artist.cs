using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Artist
    {
        public Artist(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Albums = new List<Album>();

            Artists.Add(this);
        }

        public List<Album> Albums { get; set; }

        public static List<Artist> Artists { get; set; }

        public string Name { get; set; }
    }
}