using ChiaPool.Extensions;
using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        public MinerController(MinerContext dbContext, FirewallService firewallService, PlotService plotService)
        {
            DbContext = dbContext;
            FirewallService = firewallService;
            PlotService = plotService;
        }

        [HttpPost("Start")]
        public async Task<IActionResult> StartMinerSessionAsync([FromHeader(Name = "Authorization")] string token)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

            if (miner == null)
            {
                return Unauthorized();
            }

            var minerAddress = GetRequestIP();
            if (minerAddress == null)
            {
                return NotFound();
            }
            if (!minerAddress.Equals(miner.LastAddress))
            {
                await FirewallService.SwapMinerIP(miner.LastAddress, minerAddress);
                miner.LastAddress = minerAddress;
            }

            await DbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("Claim")] //Called once every minute by each miner
        public async Task<IActionResult> ClaimMiningTimeAsync([FromHeader(Name = "Authorization")] string token, [FromForm] short activePlots)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

            if (miner == null)
            {
                return Unauthorized();
            }

            if (miner.NextIncrement > DateTimeOffset.UtcNow)
            {
                return Conflict();
            }

            var minerAddress = GetRequestIP();
            if (minerAddress == null)
            {
                return NotFound();
            }
            if (!minerAddress.Equals(miner.LastAddress))
            {
                await FirewallService.SwapMinerIP(miner.LastAddress, minerAddress);
                miner.LastAddress = minerAddress;
            }

            miner.LastPlotCount = activePlots;
            miner.PlotMinutes += activePlots;
            miner.NextIncrement = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(50); //10 second bufffer

            await DbContext.SaveChangesAsync();
            PlotService.IncrementTotalPlotMinutes(activePlots);

            return Ok();
        }

        [HttpGet("Get/Token/{token}")]
        public async Task<IActionResult> GetMinerInfoByTokenAsync([FromRoute] string token)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

            return miner == null
                ? NotFound()
                : Ok(miner.WithoutSecrets());
        }

        [HttpGet("List/Name/{name}")]
        public async Task<IActionResult> GetMinerListByNameAsync([FromRoute] string name)
        {
            var miners = await DbContext.Miners
                .Where(x => x.Owner.Name == name)
                .ToListAsync();

            return miners == null
                ? NotFound()
                : Ok(miners.Select(x => x.WithoutSecrets()));
        }
        [HttpGet("List/Id/{id}")]
        public async Task<IActionResult> GetMinerListByIdAsync([FromRoute] long id)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Owner.Id == id);

            return miner == null
                ? NotFound()
                : Ok(miner.WithoutSecrets());
        }

        private IPAddress GetRequestIP()
                => !Request.Headers.TryGetValue("HTTP_X-FORWARDED-FOR", out var value)
                    ? null
                    : IPAddress.Parse(value);
    }
}
