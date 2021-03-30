using ChiaMiningManager.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChiaMiningManager.Api
{
    public class ServerApiAccessor : ApiAccessor
    {
        private const string ApiUrl = "https://pool.playwo.de";

        public ServerApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<List<Miner>> GetMinersAsync()
            => Client.GetFromJsonAsync<List<Miner>>(Routes.ListMiners(ApiUrl));

        public Task<Miner> GetMinerByIdAsync(Guid id)
            => Client.GetFromJsonAsync<Miner>(Routes.GetMinerById(ApiUrl, id));
        public Task<Miner> GetMinerByNameAsync(string name)
            => Client.GetFromJsonAsync<Miner>(Routes.GetMinerByName(ApiUrl, name));
    }
}
