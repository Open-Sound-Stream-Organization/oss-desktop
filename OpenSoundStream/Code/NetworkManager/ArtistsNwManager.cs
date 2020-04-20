using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.NetworkManager
{
    class ArtistsNwManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

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
            return artist;
        }

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
