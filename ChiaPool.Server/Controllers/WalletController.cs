using ChiaPool.Models;
using ChiaPool.Services;
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
        private readonly HashingService HashService;

        public WalletController(MinerContext dbContext, WalletService walletService, HashingService hashService)
        {
            DbContext = dbContext;
            WalletService = walletService;
            HashService = hashService;
        }

        [HttpGet("Get/User/{name}/{password}")]
        public async Task<IActionResult> GetWalletAsync([FromRoute] string name, [FromRoute] string password)
        {
            string passwordHash = HashService.HashString(password);
            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Name == name && x.PasswordHash == passwordHash);

            if (user == null)
            {
                return Unauthorized();
            }

            long totalPlotMinutes = await DbContext.Miners
                .Where(x => x.OwnerId == user.Id)
                .SumAsync(x => x.PlotMinutes);

            var wallet = await WalletService.GetWalletFractionAsync(totalPlotMinutes);
            return Ok(wallet);
        }

        [HttpGet("Get/Pool")]
        public async Task<IActionResult> GetPoolWalletAsync([FromHeader(Name = "Authorization")] string token)
        {
            if (!await DbContext.Miners.AnyAsync(x => x.Token == token))
            {
                return Unauthorized();
            }

            var wallet = await WalletService.GetWalletAsync();
            return Ok(wallet);
        }
    }
}
