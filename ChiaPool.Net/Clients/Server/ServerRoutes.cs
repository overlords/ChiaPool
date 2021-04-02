using System;
using System.IO.Pipes;

namespace ChiaPool.Api
{
    internal static class ServerRoutes
    {
        public static Uri Status(string apiUrl)
            => new Uri(apiUrl + "Status");

        public static Uri ListUsers(string apiUrl)
            => new Uri(apiUrl + "User/List");
        public static Uri GetUserByName(string apiUrl, string name)
            => new Uri(apiUrl + $"User/Get/Name/{name}");
        public static Uri GetUserById(string apiUrl, long id)
            => new Uri(apiUrl + $"User/Get/Id/{id}");

        public static Uri ListMinersByName(string apiUrl, string name)
            => new Uri(apiUrl + $"Miner/List/Name/{name}");
        public static Uri ListMinersById(string apiUrl, long id)
            => new Uri(apiUrl + $"Miner/List/Id/{id}");
        public static Uri GetMinerByToken(string apiUrl, string token)
            => new Uri(apiUrl + $"Miner/Get/Token/{token}");

        public static Uri GetWalletByAccount(string apiUrl, string name, string password)
            => new Uri(apiUrl + $"Wallet/Get/User/{name}/{password}");
        public static Uri GetPoolWallet(string apiUrl)
            => new Uri(apiUrl + $"Wallet/Get/Pool");

        public static Uri GetPoolInfo(string apiUrl)
            => new Uri(apiUrl + $"Pool/Info");
    }
}
