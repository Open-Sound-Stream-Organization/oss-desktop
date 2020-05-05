using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.NetworkManager
{
    public class ApiKeyManager
    {
        private static HttpClient client = NetworkHandler.GetClient();

        public static ApiKey GetApiKey()
        {
            ApiKey api = new ApiKey();
            api.purpose = "Desktop Session, " + DateTime.Now.ToString();

            var responseTask = client.PostAsJsonAsync("apikey", api);
            responseTask.Wait();

            var result = responseTask.Result;
            ApiKey apiKey = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<ApiKey>();
                readTask.Wait();

                apiKey = readTask.Result;
            }

            return apiKey;
        }

        public static List<ApiKey> GetApiKeys()
        {
            var responseTask = client.GetAsync("apikey");
            responseTask.Wait();

            var result = responseTask.Result;
            IDictionary<string, dynamic> json = null;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IDictionary<string, dynamic>>();
                readTask.Wait();
                json = readTask.Result;
            }
            List<ApiKey> apiKeys = JsonConvert.DeserializeObject<List<Playlist>>(json["objects"].ToString());

            return apiKeys;
        }

        public static void DeleteApiKey(int id)
        {
            //Http Delete
            var deleteTask = client.DeleteAsync("apikey/" + id + "/");
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
            }

        }
    }
}
