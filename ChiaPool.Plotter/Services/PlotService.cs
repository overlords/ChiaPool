using ChiaPool.Configuration;
using ChiaPool.Models;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class PlotService : Service
    {
        [Inject]
        private readonly AccessOption AccessOptions;
        [Inject]
        private readonly PlotStorageService PlotStorageService;

        protected override ValueTask InitializeAsync()
        {
            return base.InitializeAsync();
        }
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

            return new RemotePlot(storedPlot.Id, $"{AccessOptions}/Plot/Download/{storedPlot.Secret}");
        }

        public async Task DeletePlotAsync(long plotId)
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PlotContext>();

            var storedPlot = await dbContext.StoredPlots
                .FirstOrDefaultAsync(x => x.Id == plotId);

            if (storedPlot != null)
            {
                dbContext.StoredPlots.Remove(storedPlot);
            }

            await dbContext.SaveChangesAsync();
            PlotStorageService.DeletePlot(plotId);
        }

        public async Task<long> GetFreePlotIdAsync()
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<PlotContext>();

            long highestPlotId = await dbContext.StoredPlots
                .MaxAsync(x => x.Id);

            return highestPlotId + 1;
        }
    }
}
