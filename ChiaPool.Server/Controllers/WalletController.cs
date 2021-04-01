using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("Get/Token/{token}")]
        public async Task<IActionResult> GetMinerWalletByTokenAsync([FromRoute] string token)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

            if (miner == null)
            {
                return Unauthorized();
            }

            var wallet = await WalletService.GetMinerWalletAsync(miner);
            return Ok(wallet);
        }

        [HttpGet("Get/Pool")]
        public async Task<IActionResult> GetPoolWalletAsync([FromHeader(Name = "Authorization")] string token)
        {
            if (!await DbContext.Miners.AnyAsync(x => x.Token == token))
            {
                return Unauthorized();
            }

            var wallet = await WalletService.GetPoolWalletAsync();
            return Ok(wallet);
        }
    }
}
