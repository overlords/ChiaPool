using System;

namespace ChiaMiningManager.Api
{
    public static class ClientRoutes
    {
        public static Uri ListPlots(string baseUrl)
            => new Uri(baseUrl + "Plots/List");
        public static Uri DeletePlotById(string baseUrl)
            => new Uri(baseUrl + "Plots/DeleteId");
        public static Uri DeletePlotByName(string baseUrl)
            => new Uri(baseUrl + "Plots/DeleteName");
    }
}
