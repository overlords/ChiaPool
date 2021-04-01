using Chia.NET.Clients;
using Chia.NET.Models;
using ChiaPool.Extensions;
using ChiaPool.Models;
using Common.Services;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class WalletService : Service
    {
        [Inject] private readonly WalletClient WalletClient;
        [Inject] private readonly PlotService PlotService;

        public Task<Wallet> GetPoolWalletAsync()
            => WalletClient.GetWalletBalance((int)ChiaWalletId.Wallet);

        public async Task<Wallet> GetMinerWalletAsync(Miner miner)
        {
            if (miner.PlotMinutes <= 0)
            {
                return new Wallet();
            }

            var poolWallet = await WalletClient.GetWalletBalance((int)ChiaWalletId.Wallet);
            var minerWallet = poolWallet.GetFraction(miner.PlotMinutes / PlotService.GetTotalPlotMinutes());
            return minerWallet;
        }
    }
}
