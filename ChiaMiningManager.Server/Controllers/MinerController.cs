using ChiaMiningManager.Models;
using ChiaMiningManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ChiaMiningManager.Controllers
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
            if (minerAddress != miner.Address)
            {
                await FirewallService.SwapMinerIP(miner.Address, minerAddress);
                miner.Address = minerAddress;
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
            if (minerAddress != miner.Address)
            {
                await FirewallService.SwapMinerIP(miner.Address, minerAddress);
                miner.Address = minerAddress;
            }

            miner.PlotMinutes += activePlots;
            miner.NextIncrement = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(50); //10 second bufffer

            await DbContext.SaveChangesAsync();

            return Ok();
        }

        private IPAddress GetRequestIP()
            => !Request.Headers.TryGetValue("HTTP_X_FORWARDED_FOR", out var value)
                ? null
                : IPAddress.Parse(value);
    }
}
