using OpenSoundStream.Code.DataManager;
using OpenSoundStream.Code.NetworkManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Playlist : PlayableContainer
    {
        public int? id { get; set; } 
        public string resource_uri { get; set; }
        public string[] tags { get; set; }
        public Playlist(string Name) : base()
        {
            name = Name ?? throw new ArgumentNullException(nameof(Name));
            Tracks = new LinkedList<Track>();
        }

        public void initializePlaylist()
        {
            PlaylistsNwManager.PostPlaylist(this);
            PlaylistsManager.db_Add_Update_Record(PlaylistsNwManager.GetPlaylists().FindLast(e => e.name == this.name));
            Playlist temp = PlaylistsManager.db_GetAllPlaylists().FindLast(e => e.name == this.name);
            this.id = temp.id;
            this.resource_uri = temp.resource_uri;
        }
        //~Playlist()
        //{
        //    throw new System.NotImplementedException();
        //}

        public void AddTrack(Track track)
        {
            this.Tracks.AddLast(track);
            TrackInPlaylistManager.db_Add_Update_Record(track.id, this.id);
        }

        public void AddTrackAfterTrack(Track newTrack, Track existingTrack)
        {
            LinkedListNode<Track> node = Tracks.Find(existingTrack);
            this.Tracks.AddAfter(node, newTrack);

        }

        public void RemoveTrack(Track track)
        {
            this.Tracks.Remove(track);
            TrackInPlaylistManager.db_Delete_Record(track.id, this.id);
        }

        public void Delete()
        {
            PlaylistsManager.db_Delete_Record(this.id);
        }
    }
}