using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Playlist : PlayableContainer
    {
        public int? id { get; set; } 
        public Uri resource_uri { get; set; }
        public string[] tags { get; set; }
        public Playlist(string Name) : base()
        {
            name = Name ?? throw new ArgumentNullException(nameof(Name));
            Tracks = new LinkedList<Track>();

            Playlists.Add(this);
        }

        //~Playlist()
        //{
        //    throw new System.NotImplementedException();
        //}

        public static List<Playlist> Playlists { get; set; }

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