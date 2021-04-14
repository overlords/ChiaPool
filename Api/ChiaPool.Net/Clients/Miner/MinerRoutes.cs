using System;

namespace ChiaPool.Api
{
    internal static class MinerRoutes
    {
        public static Uri Status()
            => new Uri("Status", UriKind.Relative);


        public static Uri ListPlots()
            => new Uri("Plot/List", UriKind.Relative);
        public static Uri ReloadPlots()
            => new Uri("Plot/Reload", UriKind.Relative);
        public static Uri DeletePlotByPublicKey()
            => new Uri("Plot/DeleteKey", UriKind.Relative);
        public static Uri DeletePlotByFileName()
            => new Uri("Plot/DeleteFile", UriKind.Relative);
        public static Uri StartPlotGeneration()
            => new Uri("Plot/Generate", UriKind.Relative);

        public static Uri GetChiaLog(ushort count)
            => new Uri($"Log/Chia/{count}", UriKind.Relative);
        public static Uri GetPoolLog(ushort count)
            => new Uri($"Log/Pool/{count}", UriKind.Relative);

        public static Uri GetCurrentWalletAsync()
            => new Uri("Server/Wallet/Get/Current", UriKind.Relative);
        public static Uri GetPoolWalletAsync()
            => new Uri("Server/Wallet/Get/Pool", UriKind.Relative);

        public static Uri GetCurrentUser()
            => new Uri("Server/User/Get/Current", UriKind.Relative);
        public static Uri GetCurrentMiner()
            => new Uri("Server/Miner/Get/Current", UriKind.Relative);
        public static Uri ListOwnedMiners()
            => new Uri("Server/Miner/List/Current", UriKind.Relative);
        public static Uri ListOwnedPlotters()
            => new Uri("Server/Plotter/List/Current", UriKind.Relative);
    }
}
