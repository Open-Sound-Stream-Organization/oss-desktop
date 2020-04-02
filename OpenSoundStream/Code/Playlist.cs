using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Playlist
    {
        public Playlist()
        {
            throw new System.NotImplementedException();
        }

        ~Playlist()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.Queue<OpenSoundStream.Track> Tracks { get; set; }

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