using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Album : PlayableContainer
    {
        public Album(string Name) : base()
        {
            name = Name ?? throw new ArgumentNullException(nameof(Name));
            Tracks = new LinkedList<Track>();

            Albums.Add(this);
        }

        public static List<Album> Albums { get; set; }

        public Artist[] Artist { get; set; }

        public int? id { get; set; }
    }
}