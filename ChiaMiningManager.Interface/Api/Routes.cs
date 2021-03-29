using System;

namespace ChiaMiningManager.Models
{
    public static class Routes
    {
        public static Uri ListPlots(string baseUrl)
            => new Uri(baseUrl + "/Plots/List");

        public static Uri DeletePlotId(string baseUrl)
            => new Uri(baseUrl + "/Plots/DeleteId");

        public static Uri DeletePlotName(string baseUrl)
            => new Uri(baseUrl + "/Plots/DeleteName");
    }
}
