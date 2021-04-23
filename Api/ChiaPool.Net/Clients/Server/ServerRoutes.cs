using System;

namespace ChiaPool.Api
{
    internal static class ServerRoutes
    {
        public static Uri Status()
            => new Uri("Status", UriKind.Relative);

        public static Uri RegisterUser()
            => new Uri("User/Register", UriKind.Relative);

        public static Uri ListUsers()
            => new Uri("User/List", UriKind.Relative);
        public static Uri GetUserByName(string name)
            => new Uri($"User/Get/Name/{name}", UriKind.Relative);
        public static Uri GetUserById(long id)
            => new Uri($"User/Get/Id/{id}", UriKind.Relative);

        public static Uri CreateMiner()
            => new Uri("Miner/Create", UriKind.Relative);
        public static Uri ListMinersByOwnerName(string name)
            => new Uri($"Miner/List/Name/{name}", UriKind.Relative);
        public static Uri ListMinersByOwnerId(long id)
            => new Uri($"Miner/List/Id/{id}", UriKind.Relative);
        public static Uri GetMinerById(long id)
            => new Uri($"Miner/Get/Id/{id}", UriKind.Relative);
        public static Uri GetMinerByName(string name)
            => new Uri($"Miner/Get/Name/{name}", UriKind.Relative);
        public static Uri GetMinerByToken(string token)
            => new Uri($"Miner/Get/Token/{token}", UriKind.Relative);

        public static Uri CreatePlotter()
            => new Uri("Plotter/Create", UriKind.Relative);
        public static Uri ListPlottersByOwnerName(string name)
            => new Uri($"Plotter/List/Name/{name}", UriKind.Relative);
        public static Uri ListPlottersByOwnerId(long id)
            => new Uri($"Plotter/List/Id/{id}", UriKind.Relative);
        public static Uri GetPlotterById(long id)
            => new Uri($"Plotter/Get/Id/{id}", UriKind.Relative);
        public static Uri GetPlotterByName(string name)
            => new Uri($"Plotter/Get/Name/{name}", UriKind.Relative);
        public static Uri GetPlotterByToken(string token)
            => new Uri($"Plotter/Get/Token/{token}", UriKind.Relative);

        public static Uri GetWalletByOwnerId(long userId)
            => new Uri($"Wallet/Get/User/Id/{userId}", UriKind.Relative);
        public static Uri GetWalletByOwnerName(string name)
            => new Uri($"Wallet/Get/User/Name/{name}", UriKind.Relative);
        public static Uri GetPoolWallet()
            => new Uri($"Wallet/Get/Pool", UriKind.Relative);
        public static Uri GetPoolWalletAddress()
            => new Uri("Wallet/Get/Address", UriKind.Relative);

        public static Uri GetPoolInfo()
            => new Uri($"Pool/Info", UriKind.Relative);

        public static Uri GetPlotTransferPrice(int deadlineHours)
            => new Uri($"Plot/Transfer/Cost/{deadlineHours}", UriKind.Relative);
        public static Uri BuyPlotTransfer(int deadlineHours)
            => new Uri($"Plot/Transfer/Buy/{deadlineHours}", UriKind.Relative);

        public static Uri GetPlottingKeys()
            => new Uri("Keys/Plotting", UriKind.Relative);

    }
}
