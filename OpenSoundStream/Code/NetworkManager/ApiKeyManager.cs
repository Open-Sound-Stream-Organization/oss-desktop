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
    }
}
