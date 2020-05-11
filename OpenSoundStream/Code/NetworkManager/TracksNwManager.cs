using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace OpenSoundStream.Code.NetworkManager
{
    class TracksNwManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

        private static HttpClient dlClient = NetworkHandler.GetDlClient();

        /// <summary>
        /// Get all tracks from serverDb
        /// </summary>
        /// <returns></returns>
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

                // < Audio download >
                var httpResponseMessage = dlClient.GetAsync("song_file/" + track.id + "/");
                httpResponseMessage.Wait();

                var resp = httpResponseMessage.Result;
                if (resp.IsSuccessStatusCode)
                {
                    System.Net.Http.HttpContent content = resp.Content;
                    string format = content.Headers.ContentDisposition.FileName.Split('.')[content.Headers.ContentDisposition.FileName.Split('.').Length - 1];
                    format = format.Replace("\"", "");

                    if( format == "wav"|| format == "mp3")
                    {
                        Stream contentStream = content.ReadAsStreamAsync().Result; // get the actual content stream

                        string path = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath + "/Data/Tracks/" + track.title + "." + format;

                        using (FileStream fileStream = File.Create(path))
                        {
                            contentStream.Seek(0, SeekOrigin.Begin);
                            contentStream.CopyTo(fileStream);
                            track.audio = path;
                        }
                    }
                }
                // </ Audio download >
            }
            return tracks;
        }

        /// <summary>
        /// Get track from serverDb
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

            // < audio download >
            var httpResponseMessage = dlClient.GetAsync("song_file/" + id + "/");
            httpResponseMessage.Wait();

            var resp = httpResponseMessage.Result;
            if (resp.IsSuccessStatusCode)
            {
                HttpContent content = resp.Content;
                Stream contentStream = content.ReadAsStreamAsync().Result; // get the actual content stream

                string path = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath + "/Data/Tracks/" + track.title + ".mp3";

                using (FileStream fileStream = File.Create(path))
                {
                    contentStream.Seek(0, SeekOrigin.Begin);
                    contentStream.CopyTo(fileStream);
                    track.audio = path;
                }
            }
            // </ audio download >

            return track;
        }

        /// <summary>
        /// Add a new track
        /// </summary>
        /// <param name="track"></param>
        public static void PostTrack(Track track)
        {
            Dictionary<string, dynamic> dic = new Dictionary<string, dynamic>();
            dic.Add("title", track.title);
            dic.Add("album", track.album);
            dic.Add("artists", track.artists);
            //HTTP Post
            var postTask = client.PostAsJsonAsync("track/", dic);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        /// <summary>
        /// Update track
        /// </summary>
        /// <param name="track"></param>
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


        /// <summary>
        /// Delete track
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// Add audio to track on serverDb
        /// </summary>
        /// <param name="track"></param>
        public static void PutAudio (Track track)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(File.Open(track.audio, FileMode.Open)), "audio", track.title);

            var httpResponseMessage = client.PutAsync("song/" + track.id + "/", content);
            httpResponseMessage.Wait();

            var resp = httpResponseMessage.Result;
            if (resp.IsSuccessStatusCode)
            {
            }
        }
    }
}
