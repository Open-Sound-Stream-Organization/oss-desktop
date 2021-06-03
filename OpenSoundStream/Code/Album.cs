using OpenSoundStream.Code.DataManager;
using OpenSoundStream.Code.NetworkManager;
using System;
using System.Collections.Generic;

namespace OpenSoundStream
{
    public class Album : PlayableContainer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name"></param>
        public Album(string Name) : base()
        {
            name = Name ?? throw new ArgumentNullException(nameof(Name));
            Tracks = new LinkedList<Track>();
        }

        /// <summary>
        /// Sync a new Album with localDb and serverDb
        /// </summary>
        public void initializeAlbum()
        {
            AlbumsNwManager.PostAlbum(this);
            AlbumsManager.db_Add_Update_Record(AlbumsNwManager.GetAlbums().FindLast(e => e.name == this.name));
            Album temp = AlbumsManager.db_GetAllAlbums().FindLast(e => e.name == this.name);
            this.id = temp.id;
            this.resource_uri = temp.resource_uri;
        }

        #region Properties

        public Artist[] Artist { get; set; }
        public DateTime? release { get; set; }
        public int? id { get; set; }
        public string mbid { get; set; }
        public string[] artists { get; set; }
        public string[] songs { get; set; }
        public string[] tags { get; set; }
        public string cover_file { get; set; }
        public string cover_url { get; set; }
        public string resource_uri { get; set; }

        #endregion
    }
}