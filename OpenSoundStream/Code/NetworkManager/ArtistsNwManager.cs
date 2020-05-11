using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;


namespace OpenSoundStream.Code.NetworkManager
{
    class ArtistsNwManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

        /// <summary>
        /// Get all artists from serverDb
        /// </summary>
        /// <returns></returns>
        public static List<Artist> GetArtists()
        {
            var responseTask = client.GetAsync("artist");
            responseTask.Wait();

            var result = responseTask.Result;
            IDictionary<string, dynamic> json = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IDictionary<string, dynamic>>();
                readTask.Wait();
                json = readTask.Result;
            }
            List<Artist> artists = JsonConvert.DeserializeObject<List<Artist>>(json["objects"].ToString());

            // filter ids from server path
            foreach (var artist in artists)
            {
                for (int i = 0; i < artist.songs.Length; i++)
                {
                    artist.songs[i] = artist.songs[i].Split('/')[artist.songs[i].Split('/').Length - 1];
                }
                for (int i = 0; i < artist.albums.Length; i++)
                {
                    artist.albums[i] = artist.albums[i].Split('/')[artist.albums[i].Split('/').Length - 1];
                }
            }

            return artists;
        }

        /// <summary>
        /// Get an artist from serverDb
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Artist GetArtist(int id)
        {
            var responseTask = client.GetAsync("artist/" + id + "/");
            responseTask.Wait();

            var result = responseTask.Result;
            Artist artist = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Artist>();
                readTask.Wait();

                artist = readTask.Result;
            }

            for (int i = 0; i < artist.songs.Length; i++)
            {
                artist.songs[i] = artist.songs[i].Split('/')[artist.songs[i].Split('/').Length - 1];
            }

            return artist;
        }

        /// <summary>
        /// Add new artist
        /// </summary>
        /// <param name="artist"></param>
        public static void PostArtist(Artist artist)
        {
            //HTTP Post
            var postTask = client.PostAsJsonAsync("artist/", artist);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        /// <summary>
        /// Update artist
        /// </summary>
        /// <param name="artist"></param>
        public static void PutArtist(Artist artist)
        {
            //HTTP Put
            var putTask = client.PutAsJsonAsync("artist/" + artist.id + "/", artist);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        /// <summary>
        /// Delete artist
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteArtist(int? id)
        {
            if (id == null)
            {
                return;
            }

            //Http Delete
            var deleteTask = client.DeleteAsync("artist/" + id + "/");
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }
    }
}
