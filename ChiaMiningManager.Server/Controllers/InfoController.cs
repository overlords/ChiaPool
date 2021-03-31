using Chia.NET.Clients;
using ChiaMiningManager.Extensions;
using ChiaMiningManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChiaMiningManager.Controllers
{
    [Route("Info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly WalletClient WalletClient;
        private readonly MinerContext DbContext;

        public InfoController(WalletClient walletClient, MinerContext dbContext)
        {
            WalletClient = walletClient;
            DbContext = dbContext;
        }

        [HttpGet("Wallet")]
        public async Task<IActionResult> GetWalletInfoAsync([FromHeader(Name = "Authorization")] string token)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);
            if (miner == null)
            {
                return Unauthorized();
            }
            var totalPM = await DbContext.Miners.SumAsync(x => x.PlotMinutes);

            var totalWallet = await WalletClient.GetWalletBalance(1);
            return Ok(totalWallet.MultiplyBy(miner.PlotMinutes / totalPM));
        }

        [HttpGet("Status")]
        public async Task<IActionResult> GetStatusAsync()
        {
            var status = new ServerStatus();
            return Ok(status);
        }
    }
}
