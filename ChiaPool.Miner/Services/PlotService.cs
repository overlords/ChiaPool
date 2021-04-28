using Chia.NET.Clients;
using Chia.NET.Models;
using ChiaPool.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class PlotService
    {
        private readonly HarvesterClient HarvesterClient;
        private readonly ILoggerFactory LoggerFactory;

        public PlotService(HarvesterClient harvesterClient, ILoggerFactory loggerFactory)
        {
            HarvesterClient = harvesterClient;
            LoggerFactory = loggerFactory;
        }

        public async Task<Plot[]> GetPlotsAsync()
            => await HarvesterClient.GetPlotsAsync();

        public async Task<PlotInfo[]> GetPlotInfosAsync()
        {
            var plots = await GetPlotsAsync();

            return plots.Select(x => new PlotInfo(x.PublicKey))
                        .ToArray();
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
