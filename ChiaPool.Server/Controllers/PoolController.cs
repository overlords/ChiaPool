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
        private readonly PlotService PlotService;
        private readonly PlotterService PlotterService;
        private readonly MinerService MinerService;

        public PoolController(CustomizationOption customizationOptions, MinerContext dbContext, PlotService plotService, PlotterService plotterService, MinerService minerService)
        {
            CustomizationOptions = customizationOptions;
            DbContext = dbContext;
            PlotService = plotService;
            PlotterService = plotterService;
            MinerService = minerService;
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

                ActiveMinerCount = MinerService.GetActiveMinerCount(),
                ActivePlotterCount = PlotterService.GetActivePlotterCount(),

                PlotterCapactity = PlotterService.GetPlottingCapacity(),

                PlotterPlots = PlotterService.GetAvailablePlotCount(),
                MinerPlots = MinerService.GetActivePlotCount(),

                TotalPlotMinutes = PlotService.GetTotalPlotMinutes(),
            };

            return Ok(poolInfo);
        }
    }
}
