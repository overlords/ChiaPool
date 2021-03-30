using Chia.NET.Clients;
using Chia.NET.Models;
using ChiaMiningManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!await DbContext.Miners.AnyAsync(x => x.Token == token))
            {
                return Unauthorized();
            }

            var wallet = await WalletClient.GetWalletBalance(1);
            return Ok(wallet);
        }
    }
}
