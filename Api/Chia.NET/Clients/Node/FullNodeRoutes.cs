using System;

namespace Chia.NET.Clients
{
    internal static class FullNodeRoutes
    {
        public static Uri GetBlockchainState(string apiUrl)
            => new Uri(apiUrl + "get_blockchain_state");
    }
}
