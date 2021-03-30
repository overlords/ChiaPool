using ChiaMiningManager.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChiaMiningManager.Api
{
    public class ClientApiAccessor : ApiAccessor
    {
        private const string ApiUrl = "http://localhost:8888";

        public ClientApiAccessor(HttpClient client)
            :base(client)
        {
        }

        public Task<List<Plot>> GetPlotsAsync()
            => Client.GetFromJsonAsync<List<Plot>>(Routes.ListPlots(ApiUrl));

        public Task<bool> DeletePlotByIdAsync(Guid id)
            => PostFromJsonAsync<bool>(Routes.DeletePlotById(ApiUrl), new Dictionary<string, string>()
            {
                ["id"] = id.ToString(),
            });

        public Task<bool> DeletePlotByNameAsync(string name)
            => PostFromJsonAsync<bool>(Routes.DeletePlotById(ApiUrl), new Dictionary<string, string>()
            {
                ["name"] = name,
            });

    }
}
