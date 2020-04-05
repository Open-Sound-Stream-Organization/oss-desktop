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

        //~Playlist()
        //{
        //    throw new System.NotImplementedException();
        //}

        public LinkedList<Track> Tracks { get; set; }

        public static List<Playlist> Playlists { get; set; }

        public string Name { get; set; }

        public void AddTrack(Track track)
        {
            this.Tracks.AddLast(track);
        }

        public void AddTrackAfterTrack(Track newTrack, Track existingTrack)
        {
            LinkedListNode<Track> node = Tracks.Find(existingTrack);
            this.Tracks.AddAfter(node, newTrack);
        }

        public void RemoveTrack(Track track)
        {
            this.Tracks.Remove(track);
        }

        public void Delete()
        {
            Playlists.Remove(this);
        }
    }
}