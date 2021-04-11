using Chia.NET.Clients;
using Chia.NET.Models;
using ChiaPool.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class PlotManager
    {
        private readonly HarvesterClient HarvesterClient;
        private readonly ILoggerFactory LoggerFactory;

        public PlotManager(HarvesterClient harvesterClient, ILoggerFactory loggerFactory)
        {
            HarvesterClient = harvesterClient;
            LoggerFactory = loggerFactory;
        }

        public async Task<Plot[]> GetPlotsAsync()
        {
            var plots = await HarvesterClient.GetPlotsAsync();
            foreach(var plot in plots)
            {
                plot.FileName = plot.FileName.Split('/').Last();
            }
            return plots;
        }

        public async Task<int> GetPlotCountAsync()
            => (await HarvesterClient.GetPlotsAsync()).Length;

        public async Task ReloadPlotsAsync() 
            => await HarvesterClient.RefreshPlotsAsync();

        public async Task<bool> DeletePlotByPubKeyAsync(string publicKey)
        {
            var plots = await GetPlotsAsync();
            var plotToDelete = plots.FirstOrDefault(x => x.PublicKey == publicKey);

            if (plotToDelete == null)
            {
                return false;
            }

            await HarvesterClient.DeletePlotAsync(plotToDelete.FileName);
            return true;
        }
        public async Task<bool> DeletePlotByFileNameAsync(string fileName)
        {
            var plots = await GetPlotsAsync();
            var plotToDelete = plots.FirstOrDefault(x => x.FileName.EndsWith(fileName));

            if (plotToDelete == null)
            {
                return false;
            }

            await HarvesterClient.DeletePlotAsync(plotToDelete.FileName);
            return true;
        }
        public async Task GeneratePlotAsync(PlottingConfiguration config)
        {
            var logger = LoggerFactory.CreateLogger("Plotting");
            await ShellHelper.RunPlotGenerationAsync(config, logger);
        }
    }
}
