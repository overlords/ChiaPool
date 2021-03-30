using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChiaMiningManager.Api
{
    public abstract class ApiAccessor
    {
        protected readonly HttpClient Client;

        public ApiAccessor(HttpClient client)
        {
            Client = client;
        }

        protected async Task<T> PostAsync<T>(Uri uri, IDictionary<string, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters);
            var response = await Client.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
