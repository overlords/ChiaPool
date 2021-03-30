using System;

namespace Chia.NET.Clients
{
    internal static class WalletRoutes
    {
        public static Uri GetWalletBalance(string apiUrl)
            => new Uri(apiUrl + "get_wallet_balance");
    }
}
