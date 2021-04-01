using Chia.NET.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chia.NET.Clients
{
    public sealed class WalletClient : ChiaApiClient
    {
        private const string ApiUrl = "https://localhost:9256/";

        public WalletClient()
            : base("wallet", ApiUrl)
        {
        }

        /// <summary>
        /// Retrieves balances for a wallet.
        /// </summary>
        /// <returns></returns>
        public async Task<Wallet> GetWalletBalance(int walletId)
        {
            var result = await PostAsync<GetWalletBalanceResult>(WalletRoutes.GetWalletBalance(ApiUrl), new Dictionary<string, string>()
            {
                ["wallet_id"] = $"{walletId}"
            });
            return result.Wallet;
        }
    }
}
