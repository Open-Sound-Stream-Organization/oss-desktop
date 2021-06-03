using System;
using System.Collections.Generic;

namespace OpenSoundStream
{
    public class Artist
    {
        public Artist(string Name)
        {
            name = Name ?? throw new ArgumentNullException(nameof(Name));
            Albums = new List<Album>();
        }

        public List<Album> Albums { get; set; }
        public DateTime? begin { get; set; }
        public DateTime? end { get; set; }
        public int? id { get; set; }
        public string mbid { get; set; }
        public string name { get; set; }
        public string[] albums { get; set; }
        public string resource_uri { get; set; }
        public string[] songs { get; set; }
        public string[] tags { get; set; }
        public string type { get; set; }
    }
}