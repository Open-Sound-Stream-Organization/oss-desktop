using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Album : PlayableContainer
    {
        public Album(string name) : base()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Tracks = new LinkedList<Track>();

            Albums.Add(this);
        }

        public static List<Album> Albums { get; set; }

        public Artist Artist { get; set; }
    }
}