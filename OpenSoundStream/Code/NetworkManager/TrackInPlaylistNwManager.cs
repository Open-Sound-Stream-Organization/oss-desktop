using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.NetworkManager
{
    class TrackInPlaylistNwManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

        // Geht noch nicht wegen Server
        public static TrackInPlaylist GetTrackInPlaylist(int id)
        {
            var responseTask = client.GetAsync("trackinplaylist/" + id + "/");
            responseTask.Wait();

            var result = responseTask.Result;
            TrackInPlaylist trackinplaylist = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<TrackInPlaylist>();
                readTask.Wait();

                trackinplaylist = readTask.Result;
            }

            return trackinplaylist;
        }

        public static Object GetApiKey(int id)
        {
            client.BaseAddress = new Uri("https://oss.anjomro.de/repertoire/");

            var responseTask = client.GetAsync("song_file/1/");
            responseTask.Wait();

            var result = responseTask.Result;
            Object trackinplaylist = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Object>();
                readTask.Wait();

                trackinplaylist = readTask.Result;
            }

            return trackinplaylist;
        }
    }
    class TrackInPlaylist {
        public int trackId { get; set; }
        public int playlistId { get; set; }
    }
}
