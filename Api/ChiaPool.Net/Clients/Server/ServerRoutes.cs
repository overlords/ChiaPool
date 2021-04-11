using System;

namespace ChiaPool.Api
{
    internal static class ServerRoutes
    {
        public static Uri Status(string apiUrl)
            => new Uri(apiUrl + "Status");

        public static Uri GetCACertificate(string apiUrl)
            => new Uri(apiUrl + "Cert/CA");

        public static Uri ListUsers(string apiUrl)
            => new Uri(apiUrl + "User/List");
        public static Uri GetUserByName(string apiUrl, string name)
            => new Uri(apiUrl + $"User/Get/Name/{name}");
        public static Uri GetUserById(string apiUrl, long id)
            => new Uri(apiUrl + $"User/Get/Id/{id}");

        public static Uri ListMinersByOwnerName(string apiUrl, string name)
            => new Uri(apiUrl + $"Miner/List/Name/{name}");
        public static Uri ListPlottersByOwnerName(string apiUrl, string name)
            => new Uri(apiUrl + $"Plotter/List/Name/{name}");

        public static Uri ListMinersByOwnerId(string apiUrl, long id)
            => new Uri(apiUrl + $"Miner/List/Id/{id}");
        public static Uri ListPlottersByOwnerId(string apiUrl, long id)
            => new Uri(apiUrl + $"Plotter/List/Id/{id}");

        public static Uri GetMinerById(string apiUrl, long id)
            => new Uri(apiUrl + $"Miner/Get/Id/{id}");
        public static Uri GetPlotterById(string apiUrl, long id)
            => new Uri(apiUrl + $"Plotter/Get/Id/{id}");

        public static Uri GetMinerByToken(string apiUrl, string token)
            => new Uri(apiUrl + $"Miner/Get/Token/{token}");
        public static Uri GetPlotterByToken(string apiUrl, string token)
            => new Uri(apiUrl + $"Plotter/Get/Token/{token}");

        public static Uri GetWalletByOwnerId(string apiUrl, long userId)
            => new Uri(apiUrl + $"Wallet/Get/Id/{userId}");
        public static Uri GetWalletByOwnerName(string apiUrl, string name)
            => new Uri(apiUrl + $"Wallet/Get/Name/{name}"); 
        public static Uri GetPoolWallet(string apiUrl)
            => new Uri(apiUrl + $"Wallet/Get/Pool");

        public static Uri GetPoolInfo(string apiUrl)
            => new Uri(apiUrl + $"Pool/Info");

        public static Uri GetPlotTransferPrice(string apiUrl, int deadlineHours)
            => new Uri(apiUrl + $"Plot/Transfer/Cost/{deadlineHours}");
        public static Uri BuyPlotTransfer(string apiUrl, int deadlineHours)
            => new Uri(apiUrl + $"Plot/Transfer/Buy/{deadlineHours}");

    }
}
