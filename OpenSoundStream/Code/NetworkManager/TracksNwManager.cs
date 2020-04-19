using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.NetworkManager
{
    class TracksNwManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

        public static Track GetTrack(int id)
        {
            var responseTask = client.GetAsync("track/" + id + "/");
            responseTask.Wait();

            var result = responseTask.Result;
            Track track = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Track>();
                readTask.Wait();

                track = readTask.Result;
            }
            return track;
        }

        public static void PostTrack(Track track)
        {
            //HTTP Post
            var postTask = client.PostAsJsonAsync("track/", track);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        public static void PutTrack(Track track)
        {
            //HTTP Put
            var putTask = client.PutAsJsonAsync("track/" + track.id + "/", track);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        public static void DeleteTrack(int? id)
        {
            if (id == null)
            {
                return;
            }

            //Http Delete
            var deleteTask = client.DeleteAsync("track/" + id + "/");
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }
    }
}
