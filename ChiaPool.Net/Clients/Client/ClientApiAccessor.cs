using Chia.NET.Models;
using ChiaPool.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChiaPool.Api
{
    public sealed class ClientApiAccessor : ApiAccessor
    {
        private const string ApiUrl = "http://localhost:8888/";

        public ClientApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<ClientStatus> GetStatusAsync()
            => GetAsync<ClientStatus>(ClientRoutes.Status(ApiUrl));

        public Task<PlotInfo[]> GetPlotsAsync()
            => GetAsync<PlotInfo[]>(ClientRoutes.ListPlots(ApiUrl));

        public Task<bool> DeletePlotByPublicKeyAsync(string publicKey)
            => PostAsync<bool>(ClientRoutes.DeletePlotByPublicKey(ApiUrl), new Dictionary<string, string>()
            {
                ["publicKey"] = publicKey.ToString(),
            });

        public Task<bool> DeletePlotByFileNameAsync(string fileName)
            => PostAsync<bool>(ClientRoutes.DeletePlotByFileName(ApiUrl), new Dictionary<string, string>()
            {
                ["fileName"] = fileName,
            });

        public Task<Wallet> GetCurrentWalletAsync()
            => GetAsync<Wallet>(ClientRoutes.GetCurrentWalletAsync(ApiUrl));
        public Task<Wallet> GetPoolWalletAsync()
            => GetAsync<Wallet>(ClientRoutes.GetPoolWalletAsync(ApiUrl));

        public Task<User> GetCurrentUserAync()
            => GetAsync<User>(ClientRoutes.GetCurrentUser(ApiUrl));
        public Task<Miner> GetCurrentMinerAsync()
            => GetAsync<Miner>(ClientRoutes.GetCurrentMiner(ApiUrl));
        public Task<List<Miner>> ListOwnedMinersAsync()
            => GetAsync<List<Miner>>(ClientRoutes.ListOwnedMiners(ApiUrl));
    }
}
