using Chia.NET.Models;
using ChiaPool.Models;
using System.Collections.Generic;
using System.IO.Compression;
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

        public async Task<ZipArchive> GetCACertificateArchiveAsync(string token)
        {
            var zipStream = await GetStreamAsync(ServerRoutes.GetCACertificate(ApiUrl), "Miner", token);
            return new ZipArchive(zipStream);
        }

        public Task<List<User>> ListUsersAsync()
            => GetAsync<List<User>>(ServerRoutes.ListUsers(ApiUrl));
        public Task<User> GetUserByNameAsync(string name)
            => GetAsync<User>(ServerRoutes.GetUserByName(ApiUrl, name));
        public Task<User> GetUserByIdAsync(long id)
            => GetAsync<User>(ServerRoutes.GetUserById(ApiUrl, id));

        public Task<List<MinerInfo>> ListMinersByOwnerNameAsync(string name)
            => GetAsync<List<MinerInfo>>(ServerRoutes.ListMinersByOwnerName(ApiUrl, name));
        public Task<List<PlotterInfo>> ListPlottersByOwnerNameAsync(string name)
            => GetAsync<List<PlotterInfo>>(ServerRoutes.ListPlottersByOwnerName(ApiUrl, name));

        public Task<List<MinerInfo>> ListMinersByOwnerIdAsync(long id)
            => GetAsync<List<MinerInfo>>(ServerRoutes.ListMinersByOwnerId(ApiUrl, id));
        public Task<List<PlotterInfo>> ListPlottersByOwnerIdAsync(long id)
            => GetAsync<List<PlotterInfo>>(ServerRoutes.ListPlottersByOwnerId(ApiUrl, id));

        public Task<MinerInfo> GetMinerByIdAsync(long id)
            => GetAsync<MinerInfo>(ServerRoutes.GetMinerById(ApiUrl, id));
        public Task<MinerInfo> GetMinerByNameAsync(string name)
            => GetAsync<MinerInfo>(ServerRoutes.GetMinerByName(ApiUrl, name));
        public Task<MinerInfo> GetMinerByTokenAsync(string token)
            => GetAsync<MinerInfo>(ServerRoutes.GetMinerByToken(ApiUrl, token));

        public Task<PlotterInfo> GetPlotterByIdAsync(long id)
            => GetAsync<PlotterInfo>(ServerRoutes.GetPlotterById(ApiUrl, id));
        public Task<PlotterInfo> GetPlotterByNameAsync(string name)
            => GetAsync<PlotterInfo>(ServerRoutes.GetPlotterByName(ApiUrl, name));
        public Task<PlotterInfo> GetPlotterByTokenAsync(string token)
            => GetAsync<PlotterInfo>(ServerRoutes.GetPlotterByToken(ApiUrl, token));

        public Task<Wallet> GetWalletByOwnerIdAsync(long userId)
            => GetAsync<Wallet>(ServerRoutes.GetWalletByOwnerId(ApiUrl, userId));
        public Task<Wallet> GetWalletByOwnerNameAsync(string name)
            => GetAsync<Wallet>(ServerRoutes.GetWalletByOwnerName(ApiUrl, name));
        public Task<Wallet> GetPoolWalletAsync()
            => GetAsync<Wallet>(ServerRoutes.GetPoolWallet(ApiUrl));

        public Task<PoolInfo> GetPoolInfoAsync()
            => GetAsync<PoolInfo>(ServerRoutes.GetPoolInfo(ApiUrl));

        public Task<long> GetPlotTransferCostAsync(int deadlineHours)
            => GetAsync<long>(ServerRoutes.GetPlotTransferPrice(ApiUrl, deadlineHours));
        public Task<PlotTransfer> BuyPlotTransferAsync(string token, int deadlineHours)
            => GetAsync<PlotTransfer>(ServerRoutes.BuyPlotTransfer(ApiUrl, deadlineHours), "Miner", token);
    }
}
