using Chia.NET.Clients;
using ChiaMiningManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaMiningManager.Services
{
    public class PlotManager
    {
        private readonly ConfigurationContext DbContext;
        private readonly HarvesterClient HarvesterClient;

        public PlotManager(ConfigurationContext dbContext, HarvesterClient harvesterClient)
        {
            DbContext = dbContext;
            HarvesterClient = harvesterClient;
        }

        public async Task<PlotInfo[]> GetPlotsAsync()
        {
            var plots = await HarvesterClient.GetPlotsAsync();
            var plotInfos = await DbContext.Plots.ToListAsync();

            var finalPlotInfos = new PlotInfo[plots.Length];

            for(int i = 0; i < plots.Length; i++)
            {
                var plotInfo = plotInfos.FirstOrDefault(x => x.PublicKey == plots[i].PublicKey);

                if (plotInfo == null)
                {
                    plotInfo = new PlotInfo(plots[i].PublicKey);
                    DbContext.Plots.Add(plotInfo);
                }
                else
                {
                    plotInfos.Remove(plotInfo);
                }

                finalPlotInfos[i] = plotInfo;
            }

            DbContext.RemoveRange(plotInfos);
            await DbContext.SaveChangesAsync();

            return finalPlotInfos;
        }

        public async Task<int> GetPlotsCountAsync()
            => (await HarvesterClient.GetPlotsAsync()).Length;

        public async Task IncrementPlots()
        {
            var plots = await DbContext.Plots.ToListAsync();

            foreach (var plot in plots)
            {
                plot.Minutes++;
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
