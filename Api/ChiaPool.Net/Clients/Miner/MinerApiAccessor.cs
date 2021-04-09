using Chia.NET.Models;
using ChiaPool.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChiaPool.Api
{
    public sealed class MinerApiAccessor : ApiAccessor
    {
        private const string ApiUrl = "http://localhost:8888/";

        public MinerApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<MinerStatus> GetStatusAsync()
            => GetAsync<MinerStatus>(MinerRoutes.Status(ApiUrl));

        public Task<PlotInfo[]> GetPlotsAsync()
            => GetAsync<PlotInfo[]>(MinerRoutes.ListPlots(ApiUrl));
        public Task ReloadPlotsAsync()
            => PostAsync(MinerRoutes.ReloadPlots(ApiUrl));
        public Task<bool> DeletePlotByPublicKeyAsync(string publicKey)
            => PostAsync<bool>(MinerRoutes.DeletePlotByPublicKey(ApiUrl), new Dictionary<string, string>()
            {
                ["publicKey"] = publicKey.ToString(),
            });
        public Task<bool> DeletePlotByFileNameAsync(string fileName)
            => PostAsync<bool>(MinerRoutes.DeletePlotByFileName(ApiUrl), new Dictionary<string, string>()
            {
                ["fileName"] = fileName,
            });
        public Task StartPlotGenerationAsync(PlottingConfiguration configuration)
            => PostAsync(MinerRoutes.StartPlotGeneration(ApiUrl), configuration);

        public Task<string[]> GetChiaLogAsync(ushort count)
            => GetAsync<string[]>(MinerRoutes.GetChiaLog(ApiUrl, count));
        public Task<string[]> GetPoolLogAsync(ushort count)
            => GetAsync<string[]>(MinerRoutes.GetPoolLog(ApiUrl, count));

        public Task<Wallet> GetCurrentWalletAsync()
            => GetAsync<Wallet>(MinerRoutes.GetCurrentWalletAsync(ApiUrl));
        public Task<Wallet> GetPoolWalletAsync()
            => GetAsync<Wallet>(MinerRoutes.GetPoolWalletAsync(ApiUrl));

        public Task<User> GetCurrentUserAync()
            => GetAsync<User>(MinerRoutes.GetCurrentUser(ApiUrl));

        public Task<MinerInfo> GetCurrentMinerAsync()
            => GetAsync<MinerInfo>(MinerRoutes.GetCurrentMiner(ApiUrl));
        public Task<List<MinerInfo>> ListOwnedMinersAsync()
            => GetAsync<List<MinerInfo>>(MinerRoutes.ListOwnedMiners(ApiUrl));
        public Task<List<PlotterInfo>> ListOwnedPlottersAsync()
            => GetAsync<List<PlotterInfo>>(MinerRoutes.ListOwnedPlotters(ApiUrl));
    }
}
