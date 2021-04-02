using Chia.NET.Clients;
using ChiaPool.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly FullNodeClient FullNodeClient;

        public StatusController(FullNodeClient fullNodeClient)
        {
            FullNodeClient = fullNodeClient;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetStatusAsync()
        {
            var blockchainState = await FullNodeClient.GetBlockchainStateAsync();

            var status = new ServerStatus(blockchainState.SyncState.ProgressHeight, blockchainState.SyncState.TipHeight);
            return Ok(status);
        }
    }
}
