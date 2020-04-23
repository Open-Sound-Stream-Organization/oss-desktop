﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Json;
using Newtonsoft.Json;

namespace OpenSoundStream.Code.NetworkManager
{
    public class AlbumsNwManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

        public static List<Album> GetAlbums()
        {
            var responseTask = client.GetAsync("album");
            responseTask.Wait();

            var result = responseTask.Result;
            IDictionary<string,dynamic> json = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IDictionary<string,dynamic>>();
                readTask.Wait();
                json = readTask.Result;
            }
            List<Album> albums = JsonConvert.DeserializeObject<List<Album>>(json["objects"].ToString());

            foreach (var album in albums)
            {
                for (int i = 0; i < album.songs.Length; i++)
                {
                    album.songs[i] = album.songs[i].Split('/')[album.songs[i].Split('/').Length - 1];
                }
                for (int i = 0; i < album.artists.Length; i++)
                {
                    album.artists[i] = album.artists[i].Split('/')[album.artists[i].Split('/').Length - 1];
                }
            }

            return albums;
        }
        public static Album GetAlbum(int id)
        {
            var responseTask = client.GetAsync("album/" + id + "/");
            responseTask.Wait();

            var result = responseTask.Result;
            Album album = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Album>();
                readTask.Wait();

                album = readTask.Result;
            }

            for (int i = 0; i < album.songs.Length; i++)
            {
                album.songs[i] = album.songs[i].Split('/')[album.songs[i].Split('/').Length - 1];
            }

            return album;
        }

        public static void PostAlbum(Album album)
        {
            //HTTP Post
            var postTask = client.PostAsJsonAsync("album/", album);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        public static void PutAlbum(Album album)
        {
            //HTTP Put
            var putTask = client.PutAsJsonAsync("album/" + album.id + "/", album);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }

        public static void DeleteAlbum(int? id)
        {
            if (id == null)
            {
                return;
            }

            //Http Delete
            var deleteTask = client.DeleteAsync("album/" + id + "/");
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }
        }
    }
}
