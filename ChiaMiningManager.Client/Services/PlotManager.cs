using Chia.NET.Clients;
using ChiaMiningManager.Models;
using Microsoft.EntityFrameworkCore;
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
            await RefreshDataBase();
            return await DbContext.Plots.ToArrayAsync();
        }

        public async Task<int> GetPlotsCountAsync()
            => (await HarvesterClient.GetPlotsAsync()).Length;

        public async Task IncrementPlots()
        {
            await RefreshDataBase();
            var plots = await DbContext.Plots.ToListAsync();

            foreach (var plot in plots)
            {
                plot.Minutes++;
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> DeletePlotByPubKeyAsync(string publicKey)
        {
            var plotInfo = await DbContext.Plots.FirstOrDefaultAsync(x => x.PublicKey == publicKey);

            if (plotInfo == null)
            {
                return false;
            }

            try
            {
                await HarvesterClient.DeletePlotAsync(plotInfo.FileName);
                DbContext.Remove(plotInfo);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeletePlotByFileNameAsync(string fileName)
        {
            var plotInfo = await DbContext.Plots.FirstOrDefaultAsync(x => x.FileName.EndsWith(fileName));

            if (plotInfo == null)
            {
                return false;
            }

            try
            {
                await HarvesterClient.DeletePlotAsync(plotInfo.FileName);
                DbContext.Remove(plotInfo);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task RefreshDataBase()
        {
            var plots = await HarvesterClient.GetPlotsAsync();
            var plotInfos = await DbContext.Plots.ToListAsync();

            foreach (var plot in plots)
            {
                var plotInfo = plotInfos.FirstOrDefault(x => x.PublicKey == plot.PublicKey);

                if (plotInfo == null)
                {
                    plotInfo = new PlotInfo(plot.PublicKey, plot.FileName);
                    DbContext.Plots.Add(plotInfo);
                }
                else
                {
                    plotInfo.FileName = plot.FileName;
                    plotInfos.Remove(plotInfo);
                }
            }

            DbContext.RemoveRange(plotInfos);
            await DbContext.SaveChangesAsync();
        }
    }
}
