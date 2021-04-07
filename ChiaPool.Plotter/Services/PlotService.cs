using ChiaPool.Configuration;
using ChiaPool.Models;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Plotter.Services
{
    public class PlotService : Service
    {
        [Inject]
        private readonly StorageOption StorageOptions;
        [Inject]
        private readonly AccessOption AccessOptions;

        protected override ValueTask InitializeAsync()
        {
            return base.InitializeAsync();
        }
        public int GetPlotCapacity()
            => StorageOptions.PlotDirectories.Sum(x => x.PlotCount);

        public async Task<int> GetAvailablePlotCountAsync()
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PlotContext>();

            return await dbContext.StoredPlots.CountAsync(x => x.Available);
        }

        public async Task<RemotePlot> ActivateRemotePlotAsync()
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PlotContext>();

            var storedPlot = await dbContext.StoredPlots
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefaultAsync(x => x.Available);

            if (!File.Exists(storedPlot.Path))
            {
                throw new FileNotFoundException("Plot file could not be found!");
            }

            storedPlot.Available = false;
            await dbContext.SaveChangesAsync();

            return new RemotePlot($"{AccessOptions}/Plot/Download/{storedPlot.Secret}");
        }
    }
}
