using ChiaMiningManager.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChiaMiningManager.Api
{
    public class ClientApiAccessor
    {
        private const string ApiUrl = "http://localhost:8888";

        private readonly HttpClient Client;

        public ClientApiAccessor(HttpClient client)
        {
            Client = client;
        }

        public Task<List<Plot>> GetPlotsAsync()
            => Client.GetFromJsonAsync<List<Plot>>(Routes.ListPlots(ApiUrl));

        public Task<bool> DeletePlotByIdAsync(Guid id)
            => PostFromJsonAsync<bool>(Routes.DeletePlotId(ApiUrl), new Dictionary<string, string>()
            {
                ["id"] = id.ToString(),
            });

        public Task<bool> DeletePlotByNameAsync(string name)
            => PostFromJsonAsync<bool>(Routes.DeletePlotId(ApiUrl), new Dictionary<string, string>()
            {
                ["name"] = name,
            });

        private async Task<T> PostFromJsonAsync<T>(Uri uri, IDictionary<string, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters);
            var response = await Client.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
