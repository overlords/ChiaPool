using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Miner")]
    [ApiController]
    public class MinerController : ControllerBase
    {
        private readonly MinerContext DbContext;
        private readonly FirewallService FirewallService;
        private readonly PlotService PlotService;
        private readonly PlotterService PlotterService;
        private readonly MinerService MinerService;

        public MinerController(MinerContext dbContext, FirewallService firewallService, PlotService plotService, PlotterService plotterService, MinerService minerService)
        {
            DbContext = dbContext;
            FirewallService = firewallService;
            PlotService = plotService;
            PlotterService = plotterService;
            MinerService = minerService;
        }



        [HttpPost("Start")]
        public async Task<IActionResult> StartMinerSessionAsync([FromHeader(Name = "Authorization")] string token)
        {
            //var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

            //if (miner == null)
            //{
            //    return Unauthorized();
            //}

            //var minerAddress = GetRequestIP();
            //if (minerAddress == null)
            //{
            //    return NotFound();
            //}
            //if (!minerAddress.Equals(miner.LastAddress))
            //{
            //    await FirewallService.SwapMinerIP(miner.LastAddress, minerAddress);
            //    miner.LastAddress = minerAddress;
            //}

            //await DbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("Claim")] //Called once every minute by each miner
        public async Task<IActionResult> ClaimMiningTimeAsync([FromHeader(Name = "Authorization")] string token, [FromForm] short activePlots)
        {
            //var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

            //if (miner == null)
            //{
            //    return Unauthorized();
            //}

            //if (miner.NextIncrement > DateTimeOffset.UtcNow)
            //{
            //    return Conflict();
            //}

            //var minerAddress = GetRequestIP();
            //if (minerAddress == null)
            //{
            //    return NotFound();
            //}
            //if (!minerAddress.Equals(miner.LastAddress))
            //{
            //    await FirewallService.SwapMinerIP(miner.LastAddress, minerAddress);
            //    miner.LastAddress = minerAddress;
            //}

            //miner.LastPlotCount = activePlots;
            //miner.PlotMinutes += activePlots;
            //miner.NextIncrement = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(50); //10 second bufffer

            //await DbContext.SaveChangesAsync();
            //PlotService.IncrementTotalPlotMinutes(activePlots);

            return Ok();
        }

        [HttpGet("Get/Token/{token}")]
        public async Task<IActionResult> GetMinerByTokenAsync([FromRoute] string token)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

            if (miner == null)
            {
                return NotFound();
            }

            var minerInfo = MinerService.GetMinerInfo(miner);
            return Ok(minerInfo);
        }
        [HttpGet("Get/Id/{id}")]
        public async Task<IActionResult> GetMinerListByIdAsync([FromRoute] long id)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Id == id);

            if (miner == null)
            {
                return NotFound();
            }

            var minerInfo = MinerService.GetMinerInfo(miner);
            return Ok(minerInfo);
        }

        [HttpGet("List/Name/{name}")]
        public async Task<IActionResult> ListMinersByOwnerNameAsync([FromRoute] string name)
        {
            var miners = await DbContext.Miners
                .Where(x => x.Owner.Name == name)
                .ToListAsync();

            var minerInfos = miners.Select(x => MinerService.GetMinerInfo(x));

            return Ok(minerInfos);
        }
        [HttpGet("List/Id/{id}")]
        public async Task<IActionResult> ListMinersByOwnerIdAsync([FromRoute] long id)
        {
            var miners = await DbContext.Miners
                .Where(x => x.Owner.Id == id)
                .ToListAsync();

            var minerInfos = miners.Select(x => MinerService.GetMinerInfo(x));

            return Ok(minerInfos);
        }

        private IPAddress GetRequestIP()
                => !Request.Headers.TryGetValue("HTTP_X-FORWARDED-FOR", out var value)
                    ? null
                    : IPAddress.Parse(value);
    }
}
