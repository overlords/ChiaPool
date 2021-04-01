using System;

namespace ChiaPool.Api
{
    internal static class ServerRoutes
    {
        public static Uri Status(string apiUrl)
            => new Uri(apiUrl + "Status");

        public static Uri ListMiners(string apiUrl)
            => new Uri(apiUrl + "Miner/List");
        public static Uri GetMinerById(string apiUrl, Guid id)
            => new Uri(apiUrl + $"Miner/Get/Id/{id}");
        public static Uri GetMinerByName(string apiUrl, string name)
            => new Uri(apiUrl + $"Miner/Get/Name/{name}");
        public static Uri GetMinerByToken(string apiUrl, string token)
            => new Uri(apiUrl + $"Miner/Get/Token/{token}");

        public static Uri GetWalletByToken(string apiUrl, string token)
            => new Uri(apiUrl + $"Wallet/Get/Token/{token}");
        public static Uri GetPoolWallet(string apiUrl)
            => new Uri(apiUrl + $"Wallet/Get/Pool");

        public static Uri GetPoolInfo(string apiUrl)
            => new Uri(apiUrl + $"Pool/Info");
    }
}
