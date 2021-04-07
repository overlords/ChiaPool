using ChiaPool.Configuration.Options;
using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Pool")]
    [ApiController]
    public class PoolController : ControllerBase
    {
        private readonly CustomizationOption CustomizationOptions;
        private readonly MinerContext DbContext;
        private readonly PlotterService PlotterService;

        public PoolController(CustomizationOption customizationOptions, MinerContext dbContext, PlotterService plotterService)
        {
            CustomizationOptions = customizationOptions;
            DbContext = dbContext;
            PlotterService = plotterService;
        }

        [HttpGet("Info")]
        public async Task<IActionResult> GetPoolInfoAsync()
        {
            var minimumActiveTime = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(1);

            var poolInfo = new PoolInfo()
            {
                Name = CustomizationOptions.PoolName,
                TotalMinerCount = await DbContext.Miners.CountAsync(),
                TotalPlotterCount = await DbContext.Plotters.CountAsync(),

                ActiveMinerCount = await DbContext.Miners.CountAsync(x => x.NextIncrement >= minimumActiveTime),
                ActivePlotterCount = PlotterService.GetActivePlotterCount(),

                PlotterCapactity = PlotterService.GetPlottingCapacity(),

                PlotterPlots = PlotterService.GetAvailablePlotCount(),
                MinerPlots = await DbContext.Miners.Where(x => x.NextIncrement >= minimumActiveTime).SumAsync(x => x.LastPlotCount),
            };

            return Ok(poolInfo);
        }
    }
}
