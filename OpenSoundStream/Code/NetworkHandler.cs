using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using OpenSoundStream.Code.NetworkManager;
using OpenSoundStream.Code.DataManager;
using OpenSoundStream.Code;

namespace OpenSoundStream
{
    static public class NetworkHandler
    {
        #region Variables

        static HttpClient client = new HttpClient();

        static HttpClient DownloadClient = new HttpClient();

        private static String baseUrl = "https://de0.win/api/v1/";

        private static String dlBaseUrl = "https://oss.anjomro.de/repertoire/";

        private static ApiKey ApiKey = null;

        private static string encodedLogout = "";

        #endregion

        #region Properties

        public static HttpClient GetClient()
        {
            return client;
        }

        public static HttpClient GetDlClient()
        {
            return DownloadClient;
        }

        public static string GetBaseUrl()
        {
            return baseUrl;
        }
        
        public static ApiKey GetApiKey()
        {
            return ApiKey;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Login to Server 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Login(string username, string password)
        {
            // Encoding to Base64 for server authorization
            var plainTextBytes = Encoding.UTF8.GetBytes(username +":"+password);
            string encoded = Convert.ToBase64String(plainTextBytes);
            encodedLogout = encoded;

            // Add basic authorization
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);
            client.BaseAddress = new Uri(baseUrl);
            ApiKey = ApiKeyManager.GetApiKey();

            // Remove basic authorization
            client.DefaultRequestHeaders.Remove("Authorization");

            // Use authorization with apikey instead of username and password
            initializeNetworkHandler();
        }

        /// <summary>
        /// Logout from Server
        /// </summary>
        public static void Logout()
        {
            // Remove apikey and authorizations to cut connection with server
            client.DefaultRequestHeaders.Remove("Authorization");
            DownloadClient.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + encodedLogout);
            ApiKeyManager.DeleteApiKey(ApiKey.id);
            client.DefaultRequestHeaders.Remove("Authorization");
            ApiKey = null;
        }

        /// <summary>
        /// Get NetworkHandler
        /// </summary>
        public static void initializeNetworkHandler()
        {
            //Authorization with apikey
            client.DefaultRequestHeaders.Add("Authorization", ApiKey.key);

            //initialize downloadClient
            DownloadClient.DefaultRequestHeaders.Add("Authorization", ApiKey.key);
            DownloadClient.BaseAddress = new Uri(dlBaseUrl);
            DownloadClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Sync Local DB with Server DB
        /// </summary>
        public static void SyncLocalDbWithServerDb()
        {
            // Delete all local tables
            AlbumsManager.db_Delete_All();
            ArtistsManager.db_Delete_All();
            PlaylistsManager.db_Delete_All();

            //Get all server tables
            List<Album> albums = AlbumsNwManager.GetAlbums();
            List<Artist> artists = ArtistsNwManager.GetArtists();
            List<Playlist> playlists = PlaylistsNwManager.GetPlaylists();
            List<Track> tracks = TracksNwManager.GetTracks();


            // < sync local tables with server tables >
            foreach(var item in albums)
            {
                AlbumsManager.db_Add_Update_Record(item);
            }
            foreach (var item in artists)
            {
                ArtistsManager.db_Add_Update_Record(item);
            }
            foreach (var item in playlists)
            {
                PlaylistsManager.db_Add_Update_Record(item);
            }
            foreach (var item in tracks)
            {
                TracksManager.db_Add_Update_Record(item);
            }
            // </ sync local tables with server tables >

            // < Add relations to local db >
            foreach (Album album in albums)
            {
                foreach (string artistId in album.artists)
                {
                    AlbumFromArtistManager.db_Add_Update_Record((int)album.id, Convert.ToInt32(artistId));
                }
            }

            foreach(Artist artist in artists)
            {
                foreach(string trackId in artist.songs)
                {
                    TrackFromArtistManager.db_Add_Update_Record(Convert.ToInt32(trackId), (int)artist.id);
                }
            }
            // </ Add relations to local db >
        }

        #endregion
    }
}