using ChiaMiningManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ChiaMiningManager.Services
{
    public class PlotManager
    {
        private readonly ConfigurationContext DbContext;

        public PlotManager(ConfigurationContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task<List<Plot>> GetPlotsAsync()
            => DbContext.Plots.ToListAsync();

        public Task<int> GetPlotsCountAsync()
            => DbContext.Plots.CountAsync();

        public async Task<bool> DeletePlotByIdAsync(Guid id)
        {
            var plot = await DbContext.Plots.FirstOrDefaultAsync(x => x.Id == id);

            if (plot == null)
            {
                return false;
            }

            DbContext.Plots.Remove(plot);
            File.Delete(plot.Path);
            await DbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeletePlotByNameAsync(string name)
        {
            var plot = await DbContext.Plots.FirstOrDefaultAsync(x => x.Name == name);

            if (plot == null)
            {
                return false;
            }

            DbContext.Plots.Remove(plot);
            File.Delete(plot.Path);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task IncrementPlots()
        {
            var plots = await GetPlotsAsync();

            foreach (var plot in plots)
            {
                plot.Minutes++;
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
