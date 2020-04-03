using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Playlist
    {
        public Playlist(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Tracks = new LinkedList<Track>();

            Playlists.Add(this);
        }

        ~Playlist()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.LinkedList<OpenSoundStream.Track> Tracks { get; set; }

        public static List<Playlist> Playlists { get; set; }

        public string Name { get; set; }

        public void AddTrack(Track track)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveTrack(Track track)
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}