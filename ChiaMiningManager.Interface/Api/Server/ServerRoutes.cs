using System;

namespace ChiaMiningManager.Api
{
    public static class ServerRoutes
    {
        public static Uri ListMiners(string baseUrl)
            => new Uri(baseUrl + "Miner/List");
        public static Uri GetMinerById(string baseUrl, Guid id)
            => new Uri(baseUrl + $"Miner/Get/Id/{id}");
        public static Uri GetMinerByName(string baseUrl, string name)
            => new Uri(baseUrl + $"Miner/Get/name/{name}");
        public static Uri GetWalletBalance(string baseUrl)
            => new Uri(baseUrl + "Info/Wallet");
    }
}
