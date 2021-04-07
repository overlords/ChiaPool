using ChiaPool.Api;
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

        [Inject]
        private readonly PlotterApiAccessor PlotterAccessor;

        public PlotService()
        {
        }

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

        public double GetPlotMinutePercentage(long plotMinutes)
            => Math.Round(100d * plotMinutes / TotalPlotMinutes, 2);


    }
}
