using System;

namespace ChiaPool.Api
{
    internal static class ClientRoutes
    {
        public static Uri Status(string apiUrl)
            => new Uri(apiUrl + "Info/Status");


        public static Uri ListPlots(string apiUrl)
            => new Uri(apiUrl + "Plot/List");
        public static Uri DeletePlotByPublicKey(string apiUrl)
            => new Uri(apiUrl + "Plot/DeleteKey");
        public static Uri DeletePlotByFileName(string apiUrl)
            => new Uri(apiUrl + "Plot/DeleteFile");

        public static Uri GetCurrentMiner(string apiUrl)
            => new Uri(apiUrl + "Server/Miner/Get/Current");
        public static Uri GetCurrentWalletAsync(string apiUrl)
            => new Uri(apiUrl + "Server/Wallet/Get/Current");
        public static Uri GetPoolWalletAsync(string apiUrl)
            => new Uri(apiUrl + "Server/Wallet/Get/Pool");
    }
}
