using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OpenSoundStream.Code.NetworkManager
{
    class TracksNwManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

        private static HttpClient dlClient = NetworkHandler.GetDlClient();

        public static List<Track> GetTracks()
        {
            var responseTask = client.GetAsync("track");
            responseTask.Wait();

            var result = responseTask.Result;
            IDictionary<string, dynamic> json = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IDictionary<string, dynamic>>();
                readTask.Wait();
                json = readTask.Result;
            }
            List<Track> tracks = JsonConvert.DeserializeObject<List<Track>>(json["objects"].ToString());

            foreach (var track in tracks)
            {
                string[] splitAlbumPath = track.album.Split('/');
                track.album = splitAlbumPath[splitAlbumPath.Length - 1];

                var httpResponseMessage = dlClient.GetAsync("song_file/" + track.id + "/");
                httpResponseMessage.Wait();

                var resp = httpResponseMessage.Result;
                if (resp.IsSuccessStatusCode)
                {
                    System.Net.Http.HttpContent content = resp.Content;
                    Stream contentStream = content.ReadAsStreamAsync().Result; // get the actual content stream

                    string path = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath + "/Data/Tracks/" + track.title + ".mp3";

                    using (FileStream fileStream = File.Create(path))
                    {
                        contentStream.Seek(0, SeekOrigin.Begin);
                        contentStream.CopyTo(fileStream);
                        track.audio = path;
                    }
                }
            }
            return tracks;
        }

        public static Track GetTrack(int id)
        {
            var responseTask = client.GetAsync("song/" + id + "/");
            responseTask.Wait();

            var result = responseTask.Result;
            Track track = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Track>();
                readTask.Wait();

                track = readTask.Result;
            }

            //Adapt AlbumId
            string[] splitAlbumPath = track.album.Split('/');
            track.album = splitAlbumPath[splitAlbumPath.Length - 1];

            var httpResponseMessage = dlClient.GetAsync("song_file/" + id + "/");
            httpResponseMessage.Wait();

            var resp = httpResponseMessage.Result;
            if (resp.IsSuccessStatusCode)
            {
                System.Net.Http.HttpContent content = resp.Content;
                Stream contentStream = content.ReadAsStreamAsync().Result; // get the actual content stream

                string path = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath + "/Data/Tracks/" + track.title + ".mp3";

                using (FileStream fileStream = File.Create(path))
                {
                    contentStream.Seek(0, SeekOrigin.Begin);
                    contentStream.CopyTo(fileStream);
                    track.audio = path;
                }
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
