using Chia.NET.Clients;
using Chia.NET.Models;
using ChiaPool.Extensions;
using ChiaPool.Models;
using Common.Services;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    [InitializationPriority(-10)]
    public class WalletService : Service
    {
        [Inject] private readonly WalletClient WalletClient;
        [Inject] private readonly PlotService PlotService;
        [Inject] private readonly WalletClient WalletApiClient;

        private string WalletAddress;

        protected override async ValueTask InitializeAsync()
        {
            WalletAddress = await WalletApiClient.GetWalletAddressAsync((int)ChiaWalletId.Wallet, false);
        }

        public Task<Wallet> GetWalletAsync()
            => WalletClient.GetWalletBalance((int)ChiaWalletId.Wallet);

        public string GetWalletAddress()
            => WalletAddress;

        public async Task<Wallet> GetWalletFractionAsync(long plotMinutes)
        {
            if (plotMinutes <= 0)
            {
                return Wallet.Empty;
            }

            var poolWallet = await WalletClient.GetWalletBalance((int)ChiaWalletId.Wallet);
            var userWallet = poolWallet.GetFraction(plotMinutes / PlotService.GetTotalPlotMinutes());
            return userWallet;
        }
    }
}
