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
            Title = title;
            Filepath = filepath;
            Metadata = new Metadata();

            Tracks.Add(this);
        }

        public static List<Track> Tracks { get; set; }

        public string Title { get; set; }

        public Metadata Metadata { get; set; }

        //public System.Windows.DependencyProperty Dependency { get; set; }

        public Uri Filepath { get; set; }

        public Album Album { get; set; }

        public Artist Artist { get; set; }
    }
}