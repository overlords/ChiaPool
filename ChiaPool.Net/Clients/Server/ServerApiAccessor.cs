using Chia.NET.Models;
using ChiaPool.Models;
using ChiaPool.Models.Server;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChiaPool.Api
{
    public sealed class ServerApiAccessor : ApiAccessor
    {
        private const string ApiUrl = "https://pool.playwo.de/";

        public ServerApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<ServerStatus> GetStatusAsync()
            => GetAsync<ServerStatus>(ServerRoutes.Status(ApiUrl));

        public Task<List<Miner>> GetMinersAsync()
            => GetAsync<List<Miner>>(ServerRoutes.ListMiners(ApiUrl));
        public Task<Miner> GetMinerByIdAsync(Guid id)
            => GetAsync<Miner>(ServerRoutes.GetMinerById(ApiUrl, id));
        public Task<Miner> GetMinerByNameAsync(string name)
            => GetAsync<Miner>(ServerRoutes.GetMinerByName(ApiUrl, name));
        public Task<Miner> GetMinerByTokenAsync(string token)
            => GetAsync<Miner>(ServerRoutes.GetMinerByToken(ApiUrl, token));

        public Task<Wallet> GetWalletByTokenAsync(string token)
            => GetAsync<Wallet>(ServerRoutes.GetWalletByToken(ApiUrl, token));
        public Task<Wallet> GetPoolWalletAsync(string token)
            => GetAsync<Wallet>(ServerRoutes.GetPoolWallet(ApiUrl), token);

        public Task<PoolInfo> GetPoolInfoAsync()
            => GetAsync<PoolInfo>(ServerRoutes.GetPoolInfo(ApiUrl));

    }
}
