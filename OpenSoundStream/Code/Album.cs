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
        public string[] artists { get; set; }
        public string cover_file { get; set; }
        public string cover_url { get; set; }
        public int? mbid { get; set; }
        public DateTime? release { get; set; }
        public string resource_uri { get; set; }
        public string[] songs { get; set; }
        public string[] tags { get; set; }
    }
}