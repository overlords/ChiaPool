using Chia.NET.Models;
using ChiaPool.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChiaPool.Api
{
    public sealed class MinerApiAccessor : ApiAccessor
    {
        public MinerApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<MinerStatus> GetStatusAsync()
            => GetAsync<MinerStatus>(MinerRoutes.Status());

        public Task<Plot[]> GetPlotsAsync()
            => GetAsync<Plot[]>(MinerRoutes.ListPlots());
        public Task ReloadPlotsAsync()
            => PostAsync(MinerRoutes.ReloadPlots());
        public Task<bool> DeletePlotByPublicKeyAsync(string publicKey)
            => PostAsync<bool>(MinerRoutes.DeletePlotByPublicKey(), new Dictionary<string, string>()
            {
                ["publicKey"] = publicKey.ToString(),
            });
        public Task<bool> DeletePlotByFileNameAsync(string fileName)
            => PostAsync<bool>(MinerRoutes.DeletePlotByFileName(), new Dictionary<string, string>()
            {
                ["fileName"] = fileName,
            });
        public Task StartPlotGenerationAsync(PlottingConfiguration configuration)
            => PostAsync(MinerRoutes.StartPlotGeneration(), configuration);

        public Task<string[]> GetChiaLogAsync(ushort count)
            => GetAsync<string[]>(MinerRoutes.GetChiaLog(count));
        public Task<string[]> GetPoolLogAsync(ushort count)
            => GetAsync<string[]>(MinerRoutes.GetPoolLog(count));

        public Task<Wallet> GetCurrentWalletAsync()
            => GetAsync<Wallet>(MinerRoutes.GetCurrentWalletAsync());

        public Task<User> GetCurrentUserAync()
            => GetAsync<User>(MinerRoutes.GetCurrentUser());

        public Task<MinerInfo> GetCurrentMinerAsync()
            => GetAsync<MinerInfo>(MinerRoutes.GetCurrentMiner());
        public Task<List<MinerInfo>> ListOwnedMinersAsync()
            => GetAsync<List<MinerInfo>>(MinerRoutes.ListOwnedMiners());
        public Task<List<PlotterInfo>> ListOwnedPlottersAsync()
            => GetAsync<List<PlotterInfo>>(MinerRoutes.ListOwnedPlotters());
    }
}
