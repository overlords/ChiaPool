using System;

namespace ChiaMiningManager.Api
{
    public static class ServerRoutes
    {
        public static Uri Status(string apiUrl)
            => new Uri(apiUrl + "Info/Status");
        public static Uri ListMiners(string apiUrl)
            => new Uri(apiUrl + "Miner/List");
        public static Uri GetMinerById(string apiUrl, Guid id)
            => new Uri(apiUrl + $"Miner/Get/Id/{id}");
        public static Uri GetMinerByName(string apiUrl, string name)
            => new Uri(apiUrl + $"Miner/Get/name/{name}");
        public static Uri GetWalletBalance(string apiUrl)
            => new Uri(apiUrl + "Info/Wallet");
    }
}
