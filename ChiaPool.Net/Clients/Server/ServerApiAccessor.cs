using Chia.NET.Models;
using ChiaPool.Models;
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

        public Task<List<User>> ListUsersAsync()
            => GetAsync<List<User>>(ServerRoutes.ListUsers(ApiUrl));
        public Task<User> GetUserByNameAsync(string name)
            => GetAsync<User>(ServerRoutes.GetUserByName(ApiUrl, name));
        public Task<User> GetUserByIdAsync(long id)
            => GetAsync<User>(ServerRoutes.GetUserById(ApiUrl, id));

        public Task<List<Miner>> ListMinersByNameAsync(string name)
            => GetAsync<List<Miner>>(ServerRoutes.ListMinersByName(ApiUrl, name));
        public Task<List<Miner>> ListMinersByIdAsync(long id)
            => GetAsync<List<Miner>>(ServerRoutes.ListMinersById(ApiUrl, id));
        public Task<Miner> GetMinerByTokenAsync(string token)
            => GetAsync<Miner>(ServerRoutes.GetMinerByToken(ApiUrl, token));

        public Task<Wallet> GetWalletByTokenAsync(string name, string password)
            => GetAsync<Wallet>(ServerRoutes.GetWalletByAccount(ApiUrl, name, password));
        public Task<Wallet> GetPoolWalletAsync(string token)
            => GetAsync<Wallet>(ServerRoutes.GetPoolWallet(ApiUrl), token);

        public Task<PoolInfo> GetPoolInfoAsync()
            => GetAsync<PoolInfo>(ServerRoutes.GetPoolInfo(ApiUrl));

    }
}
