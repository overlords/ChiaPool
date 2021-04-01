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

        public MinerController(MinerContext dbContext, FirewallService firewallService)
        {
            DbContext = dbContext;
            FirewallService = firewallService;
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

            return Ok();
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAllMinersAsync()
        {
            var miners = await DbContext.Miners.ToListAsync();
            miners = miners.Select(x => x.WithoutSecrets()).ToList();
            return Ok(miners);
        }

        [HttpGet("Get/Id/{id}")]
        public async Task<IActionResult> GetMinerInfoByIdAsync([FromRoute] Guid id)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Id == id);

            return miner == null
                ? NotFound()
                : Ok(miner.WithoutSecrets());
        }
        [HttpGet("Get/Name/{name}")]
        public async Task<IActionResult> GetMinerInfoByNameAsync([FromRoute] string name)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Name == name);

            return miner == null
                ? NotFound()
                : Ok(miner.WithoutSecrets());
        }
        [HttpGet("Get/Token/{token}")]
        public async Task<IActionResult> GetMinerInfoByTokenAsync([FromRoute] string token)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

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
