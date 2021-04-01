using ChiaPool.Configuration.Options;
using ChiaPool.Models;
using ChiaPool.Models.Server;
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

        public PoolController(CustomizationOption customizationOptions, MinerContext dbContext)
        {
            CustomizationOptions = customizationOptions;
            DbContext = dbContext;
        }

        [HttpGet("Info")]
        public async Task<IActionResult> GetPoolInfoAsync()
        {
            var minimumActiveTime = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(1);

            var poolInfo = new PoolInfo()
            {
                Name = CustomizationOptions.PoolName,
                TotalMinerCount = await DbContext.Miners.CountAsync(),
                TotalPlotCount = await DbContext.Miners.SumAsync(x => x.LastPlotCount),
                ActiveMinerCount = await DbContext.Miners.CountAsync(x => x.NextIncrement >= minimumActiveTime),
                ActivePlotCount = await DbContext.Miners.Where(x => x.NextIncrement >= minimumActiveTime).SumAsync(x => x.LastPlotCount),
            };

            return Ok(poolInfo);
        }
    }
}
