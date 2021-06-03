using OpenSoundStream.Code.DataManager;
using OpenSoundStream.Code.NetworkManager;
using System;
using System.Collections.Generic;

namespace OpenSoundStream
{
    public class Playlist : PlayableContainer
    {
        #region Properties

        public int? id { get; set; } 
        public string resource_uri { get; set; }
        public string[] tags { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name"></param>
        public Playlist(string Name) : base()
        {
            name = Name ?? throw new ArgumentNullException(nameof(Name));
            Tracks = new LinkedList<Track>();
        }

        #region Methods

        /// <summary>
        /// Sync new Playlist with localDb and serverDb
        /// </summary>
        public void initializePlaylist()
        {
            PlaylistsNwManager.PostPlaylist(this);
            PlaylistsManager.db_Add_Update_Record(PlaylistsNwManager.GetPlaylists().FindLast(e => e.name == this.name));
            Playlist temp = PlaylistsManager.db_GetAllPlaylists().FindLast(e => e.name == this.name);
            this.id = temp.id;
            this.resource_uri = temp.resource_uri;
        }

        /// <summary>
        /// Add track to playlist
        /// </summary>
        /// <param name="track"></param>
        public void AddTrack(Track track)
        {
            this.Tracks.AddLast(track);
            TrackInPlaylistManager.db_Add_Update_Record(track.id, this.id);
        }

        /// <summary>
        /// Add track at specific position in Playlist
        /// </summary>
        /// <param name="newTrack"></param>
        /// <param name="existingTrack"></param>
        public void AddTrackAfterTrack(Track newTrack, Track existingTrack)
        {
            LinkedListNode<Track> node = Tracks.Find(existingTrack);
            this.Tracks.AddAfter(node, newTrack);
            TrackInPlaylistManager.db_Add_Update_Record(newTrack.id, this.id);

        }

        /// <summary>
        /// Remove track from Playlist
        /// </summary>
        /// <param name="track"></param>
        public void RemoveTrack(Track track)
        {
            this.Tracks.Remove(track);
            TrackInPlaylistManager.db_Delete_Record(track.id, this.id);
        }

        /// <summary>
        /// Delete Playlist from Database
        /// </summary>
        public void Delete()
        {
            PlaylistsManager.db_Delete_Record(this.id);
            PlaylistsNwManager.DeletePlaylist(this.id);
        }

        #endregion
    }
}