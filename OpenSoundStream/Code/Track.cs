using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Track
    {
        public static List<Track> Tracks { get; set; }

        public string Title { get; set; }

        public string FullFileName { get; set; }

        public Metadata Metadata { get; set; }

        public System.Windows.DependencyProperty Dependency { get; set; }
    }
}