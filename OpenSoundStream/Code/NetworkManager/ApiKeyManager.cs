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
            Dictionary<string, string> purpose = new Dictionary<string, string>();
            purpose.Add("purpose", "Desktop Session, " + DateTime.Now.ToString());

            var responseTask = client.PostAsJsonAsync("apikey/", purpose);
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
