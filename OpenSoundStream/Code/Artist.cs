using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Artist
    {
        public List<Album> Albums { get; set; }

        public static List<Artist> Artists { get; set; }

        public string Name { get; set; }
    }
}