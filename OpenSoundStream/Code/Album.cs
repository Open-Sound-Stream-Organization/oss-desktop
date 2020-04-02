using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Album
    {
        public List<Track> Tracks { get; set; }

        public static List<Album> Albums { get; set; }

        public string Name { get; set; }
    }
}