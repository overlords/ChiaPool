using Chia.NET.Clients;
using ChiaPool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class PlotManager
    {
        private readonly ConfigurationContext DbContext;
        private readonly HarvesterClient HarvesterClient;
        private readonly ILoggerFactory LoggerFactory;

        public PlotManager(ConfigurationContext dbContext, HarvesterClient harvesterClient, ILoggerFactory loggerFactory)
        {
            DbContext = dbContext;
            HarvesterClient = harvesterClient;
            LoggerFactory = loggerFactory;
        }

        public async Task<PlotInfo[]> GetPlotsAsync()
        {
            await RefreshDataBase();
            return await DbContext.Plots.ToArrayAsync();
        }

        public async Task<int> GetPlotCountAsync()
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

        public async Task ReloadPlotsAsync()
        {
            await HarvesterClient.RefreshPlotsAsync();
            await RefreshDataBase();
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

        public async Task GeneratePlotAsync(PlottingConfiguration config)
        {
            var logger = LoggerFactory.CreateLogger("Plotting");
            var sw = new Stopwatch();
            sw.Start();

            string command = "cd chia-blockchain " +
                          "&& . ./activate " +
                          $"&& chia plots create -k {config.Size} -d {config.Path} -t {config.CachePath} -u {config.BucketCount} -b {config.BufferSize}";

            int exitCode = await ShellHelper.RunBashAsync(command, logger);

            sw.Stop();

            if (exitCode == 0)
            {
                logger.LogInformation($"Finished plotting process after {sw.Elapsed.TotalMinutes} minutes");
            }
            else
            {
                logger.LogError($"Plotting failed after {sw.Elapsed.TotalMinutes} minutes");
            }
        }
    }
}
