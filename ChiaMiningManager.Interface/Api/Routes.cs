using System;

namespace ChiaMiningManager.Models
{
    public static class Routes
    {
        public static Uri ListPlots(string baseUrl)
            => new Uri(baseUrl + "/Plots/List");
        public static Uri DeletePlotById(string baseUrl)
            => new Uri(baseUrl + "/Plots/DeleteId");
        public static Uri DeletePlotByName(string baseUrl)
            => new Uri(baseUrl + "/Plots/DeleteName");

        public static Uri ListMiners(string baseUrl)
            => new Uri(baseUrl + "/Miner/List");
        public static Uri GetMinerById(string baseUrl, Guid id)
            => new Uri(baseUrl + $"/Miner/Get/Id/{id}");        
        public static Uri GetMinerByName(string baseUrl, string name)
            => new Uri(baseUrl + $"/Miner/Get/name/{name}");

    }
}
