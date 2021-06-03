using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.NetworkManager
{
    class PlaylistsNwManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

        /// <summary>
        /// Get all playlists from serverDb
        /// </summary>
        /// <returns></returns>
        public static List<Playlist> GetPlaylists()
        {
            var responseTask = client.GetAsync("playlist");
            responseTask.Wait();

            var result = responseTask.Result;
            IDictionary<string, dynamic> json = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IDictionary<string, dynamic>>();
                readTask.Wait();
                json = readTask.Result;
            }
            List<Playlist> playlists = JsonConvert.DeserializeObject<List<Playlist>>(json["objects"].ToString());

            return playlists;
        }

        /// <summary>
        /// Get a playlist from serverDb
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Playlist GetPlaylist(int id)
        {
            var responseTask = client.GetAsync("playlist/" + id + "/");
            responseTask.Wait();

            var result = responseTask.Result;
            Playlist playlist = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Playlist>();
                readTask.Wait();

                playlist = readTask.Result;
            }
            return playlist;
        }

        /// <summary>
        /// Add new playlist
        /// </summary>
        /// <param name="playlist"></param>
        public static void PostPlaylist(Playlist playlist)
        {
            //HTTP Post
            var postTask = client.PostAsJsonAsync("playlist/", playlist);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        /// <summary>
        /// Update playlist
        /// </summary>
        /// <param name="playlist"></param>
        public static void PutPlaylist(Playlist playlist)
        {
            //HTTP Put
            var putTask = client.PutAsJsonAsync("playlist/" + playlist.id + "/", playlist);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        /// <summary>
        /// Delete playlist
        /// </summary>
        /// <param name="id"></param>
        public static void DeletePlaylist(int? id)
        {
            if (id == null)
            {
                return;
            }

            //Http Delete
            var deleteTask = client.DeleteAsync("playlist/" + id + "/");
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }
    }
}
