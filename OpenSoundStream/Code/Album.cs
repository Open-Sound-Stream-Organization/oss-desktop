using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Album
    {
        public Album(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Tracks = new List<Track>();

            Albums.Add(this);
        }

        public static List<Album> Albums { get; set; }

        public List<Track> Tracks { get; set; }

        public string Name { get; set; }
    }
}