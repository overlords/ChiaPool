using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly MinerContext DbContext;
        private readonly WalletService WalletService;

        public WalletController(MinerContext dbContext, WalletService walletService)
        {
            DbContext = dbContext;
            WalletService = walletService;
        }

        [HttpGet("Get/Address")]
        [Authorize(AuthenticationSchemes = "Miner")]
        public Task<IActionResult> GetWalletAddressAsync()
        {
            string walletAddress = WalletService.GetWalletAddress();
            return Task.FromResult((IActionResult)Ok(walletAddress));
        }

        [HttpGet("Get/User/Id/{id}")]
        public async Task<IActionResult> GetWalletByOwnerIdAsync(long userId)
        {
            long totalPlotMinutes = await DbContext.Users
                .Where(x => x.Id == userId)
                .Select(x => x.PlotMinutes)
                .FirstOrDefaultAsync();

            var wallet = await WalletService.GetWalletFractionAsync(totalPlotMinutes);
            return Ok(wallet);
        }

        [HttpGet("Get/User/Name/{name}")]
        public async Task<IActionResult> GetWalletByOwnerNameAsync(string name)
        {
            long totalPlotMinutes = await DbContext.Users
                .Where(x => x.Name == name)
                .Select(x => x.PlotMinutes)
                .FirstOrDefaultAsync();

            var wallet = await WalletService.GetWalletFractionAsync(totalPlotMinutes);
            return Ok(wallet);
        }

        [HttpGet("Get/Pool")]
        public async Task<IActionResult> GetPoolWalletAsync()
        {
            var wallet = await WalletService.GetWalletAsync();
            return Ok(wallet);
        }
    }
}
