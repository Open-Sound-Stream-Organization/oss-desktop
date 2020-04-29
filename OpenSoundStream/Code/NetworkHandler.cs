using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Json;
using OpenSoundStream.Code.NetworkManager;
using OpenSoundStream.Code.DataManager;

namespace OpenSoundStream
{
    public class NetworkHandler
    {
        static HttpClient client = new HttpClient();

        static HttpClient DownloadClient = new HttpClient();

        private static String baseUrl = "https://de0.win/api/v1/";

        private static String dlBaseUrl = "https://oss.anjomro.de/repertoire/";

        public NetworkHandler()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "Basic", Convert.ToBase64String(
            System.Text.ASCIIEncoding.ASCII.GetBytes(
               $"{"testuser"}:{"testuser"}")));
            client.BaseAddress = new Uri(baseUrl);

            DownloadClient.DefaultRequestHeaders.Add("Authorization", "Test-API-Key");
            DownloadClient.BaseAddress = new Uri(dlBaseUrl);
            DownloadClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

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

        public static void SyncLocalDbWithServerDb()
        {
            List<Album> albums = AlbumsNwManager.GetAlbums();
            List<Artist> artists = ArtistsNwManager.GetArtists();
            List<Playlist> playlists = PlaylistsNwManager.GetPlaylists();
            List<Track> tracks = TracksNwManager.GetTracks();

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

            foreach(Album album in albums)
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


        }
    }
}