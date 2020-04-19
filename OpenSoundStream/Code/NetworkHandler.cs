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

namespace OpenSoundStream
{
    public class NetworkHandler
    {
        static HttpClient client = new HttpClient();

        private static String baseUrl = "https://de0.win/api/v1/";

        public NetworkHandler()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "Basic", Convert.ToBase64String(
            System.Text.ASCIIEncoding.ASCII.GetBytes(
               $"{"testuser"}:{"testuser"}")));
            client.BaseAddress = new Uri(baseUrl);
        }

        public static HttpClient GetClient()
        {
            return client;
        }

        public static string GetBaseUrl()
        {
            return baseUrl;
        }
    }
}