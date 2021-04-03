using System;

namespace ChiaPool.Api
{
    internal static class ClientRoutes
    {
        public static Uri Status(string apiUrl)
            => new Uri(apiUrl + "Status");


        public static Uri ListPlots(string apiUrl)
            => new Uri(apiUrl + "Plot/List");
        public static Uri DeletePlotByPublicKey(string apiUrl)
            => new Uri(apiUrl + "Plot/DeleteKey");
        public static Uri DeletePlotByFileName(string apiUrl)
            => new Uri(apiUrl + "Plot/DeleteFile");
        public static Uri StartPlotGeneration(string apiUrl)
            => new Uri(apiUrl + "Plot/Generate");

        public static Uri GetChiaLog(string apiUrl, ushort count)
            => new Uri(apiUrl + $"Log/Chia/{count}");
        public static Uri GetPoolLog(string apiUrl, ushort count)
            => new Uri(apiUrl + $"Log/Pool/{count}");

        public static Uri GetCurrentWalletAsync(string apiUrl)
            => new Uri(apiUrl + "Server/Wallet/Get/Current");
        public static Uri GetPoolWalletAsync(string apiUrl)
            => new Uri(apiUrl + "Server/Wallet/Get/Pool");

        public static Uri GetCurrentUser(string apiUrl)
            => new Uri(apiUrl + "Server/User/Get/Current");
        public static Uri GetCurrentMiner(string apiUrl)
            => new Uri(apiUrl + "Server/Miner/Get/Current");
        public static Uri ListOwnedMiners(string apiUrl)
            => new Uri(apiUrl + "Server/Miner/List/Current");
    }
}
