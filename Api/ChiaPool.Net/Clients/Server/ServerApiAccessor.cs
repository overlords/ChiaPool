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
        public ServerApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<ServerStatus> GetStatusAsync()
            => GetAsync<ServerStatus>(ServerRoutes.Status());

        public async Task<ZipArchive> GetCACertificateArchiveAsync(string token)
        {
            var zipStream = await GetStreamAsync(ServerRoutes.GetCACertificate(), "Miner", token);
            return new ZipArchive(zipStream);
        }

        public Task<List<User>> ListUsersAsync()
            => GetAsync<List<User>>(ServerRoutes.ListUsers());
        public Task<User> GetUserByNameAsync(string name)
            => GetAsync<User>(ServerRoutes.GetUserByName(name));
        public Task<User> GetUserByIdAsync(long id)
            => GetAsync<User>(ServerRoutes.GetUserById(id));

        public Task<List<MinerInfo>> ListMinersByOwnerNameAsync(string name)
            => GetAsync<List<MinerInfo>>(ServerRoutes.ListMinersByOwnerName(name));
        public Task<List<PlotterInfo>> ListPlottersByOwnerNameAsync(string name)
            => GetAsync<List<PlotterInfo>>(ServerRoutes.ListPlottersByOwnerName(name));

        public Task<List<MinerInfo>> ListMinersByOwnerIdAsync(long id)
            => GetAsync<List<MinerInfo>>(ServerRoutes.ListMinersByOwnerId(id));
        public Task<List<PlotterInfo>> ListPlottersByOwnerIdAsync(long id)
            => GetAsync<List<PlotterInfo>>(ServerRoutes.ListPlottersByOwnerId(id));

        public Task<MinerInfo> GetMinerByIdAsync(long id)
            => GetAsync<MinerInfo>(ServerRoutes.GetMinerById(id));
        public Task<MinerInfo> GetMinerByNameAsync(string name)
            => GetAsync<MinerInfo>(ServerRoutes.GetMinerByName(name));
        public Task<MinerInfo> GetMinerByTokenAsync(string token)
            => GetAsync<MinerInfo>(ServerRoutes.GetMinerByToken(token));

        public Task<PlotterInfo> GetPlotterByIdAsync(long id)
            => GetAsync<PlotterInfo>(ServerRoutes.GetPlotterById(id));
        public Task<PlotterInfo> GetPlotterByNameAsync(string name)
            => GetAsync<PlotterInfo>(ServerRoutes.GetPlotterByName(name));
        public Task<PlotterInfo> GetPlotterByTokenAsync(string token)
            => GetAsync<PlotterInfo>(ServerRoutes.GetPlotterByToken(token));

        public Task<Wallet> GetWalletByOwnerIdAsync(long userId)
            => GetAsync<Wallet>(ServerRoutes.GetWalletByOwnerId(userId));
        public Task<Wallet> GetWalletByOwnerNameAsync(string name)
            => GetAsync<Wallet>(ServerRoutes.GetWalletByOwnerName(name));
        public Task<Wallet> GetPoolWalletAsync()
            => GetAsync<Wallet>(ServerRoutes.GetPoolWallet());

        public Task<PoolInfo> GetPoolInfoAsync()
            => GetAsync<PoolInfo>(ServerRoutes.GetPoolInfo());

        public Task<long> GetPlotTransferCostAsync(int deadlineHours)
            => GetAsync<long>(ServerRoutes.GetPlotTransferPrice(deadlineHours));
        public Task<PlotTransfer> BuyPlotTransferAsync(string token, int deadlineHours)
            => GetAsync<PlotTransfer>(ServerRoutes.BuyPlotTransfer(deadlineHours), "Miner", token);
    }
}
