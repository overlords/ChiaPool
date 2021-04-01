using System;

namespace ChiaPool.Api
{
    public static class ClientRoutes
    {
        public static Uri Status(string apiUrl)
            => new Uri(apiUrl + "Info/Status");
        public static Uri GetCurrentMiner(string apiUrl)
            => new Uri(apiUrl + "Info/Miner");
        public static Uri ListPlots(string apiUrl)
            => new Uri(apiUrl + "Plots/List");
        public static Uri DeletePlotById(string apiUrl)
            => new Uri(apiUrl + "Plots/DeleteId");
        public static Uri DeletePlotByName(string apiUrl)
            => new Uri(apiUrl + "Plots/DeleteName");
        public static Uri ServerWalletInfo(string apiUrl)
            => new Uri(apiUrl + "Server/Info/Wallet");

    }
}
