using ChiaPool.Models;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class PlotService : Service
    {
        private long TotalPlotMinutes;

        protected override async ValueTask InitializeAsync()
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();

            TotalPlotMinutes = await dbContext.Miners.SumAsync(x => x.PlotMinutes);
        }

        public void IncrementTotalPlotMinutes(int amount)
            => Interlocked.Add(ref TotalPlotMinutes, amount);

        public double GetTotalPlotMinutes()
            => TotalPlotMinutes;

        public double GetMinerPercentage(Miner miner)
            => Math.Round(100d * miner.PlotMinutes / TotalPlotMinutes, 2);
    }
}
