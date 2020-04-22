using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Track
    {
        public Track(string title, Uri filepath)
        {
            this.title = title;
            Filepath = filepath;
            Metadata = new Metadata();

            Tracks.Add(this);
        }

        public Album Album { get; set; }
        public static List<Track> Tracks { get; set; }

        public int? id { get; set; }

        public string title { get; set; }

        public Metadata Metadata { get; set; }

        //public System.Windows.DependencyProperty Dependency { get; set; }

        public Uri Filepath { get; set; }

        public string album { get; set; }
        public string[] artists { get; set; }
        public string audio { get; set; }
        public int? mbid { get; set; }
        public string resource_uri { get; set; }
        public string[] tags { get; set; }

    }
}