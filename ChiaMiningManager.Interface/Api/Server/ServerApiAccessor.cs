using Chia.NET.Models;
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
        private const string ApiUrl = "https://pool.playwo.de/";

        public ServerApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<List<Miner>> GetMinersAsync()
            => Client.GetFromJsonAsync<List<Miner>>(ServerRoutes.ListMiners(ApiUrl));

        public Task<Miner> GetMinerByIdAsync(Guid id)
            => Client.GetFromJsonAsync<Miner>(ServerRoutes.GetMinerById(ApiUrl, id));
        public Task<Miner> GetMinerByNameAsync(string name)
            => Client.GetFromJsonAsync<Miner>(ServerRoutes.GetMinerByName(ApiUrl, name));

        public Task<Wallet> GetWalletBalanceAsync()
            => Client.GetFromJsonAsync<Wallet>(ServerRoutes.GetWalletBalance(ApiUrl));
    }
}
