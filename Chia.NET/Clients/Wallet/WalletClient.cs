using Chia.NET.Clients.Wallet;
using Chia.NET.Models;
using System.Threading.Tasks;

namespace Chia.NET.Clients
{
    public sealed class WalletClient : ChiaApiClient
    {
        private const string ApiUrl = "https://localhost:9256/";

        public WalletClient() 
            : base("wallet")
        {
        }

        /// <summary>
        /// Retrieves balances for a wallet.
        /// </summary>
        /// <returns></returns>
        public async Task<Wallet> GetWalletBalance(int walletId)
        {
            var result = await PostAsync<GetWalletBalanceResult>(WalletRoutes.GetWalletBalance(ApiUrl));
            return result.Wallet;
        }
    }
}
