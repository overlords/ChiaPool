using ChiaPool.Api;
using ChiaPool.Configuration;
using ChiaPool.Configuration.Options;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly ServerApiAccessor ServerAccessor;
        private readonly ServerOption ServerOptions;
        private readonly AuthOption AuthOptions;

        public ServerController(ServerApiAccessor serverAccessor, ServerOption serverOptions, AuthOption authOptions)
        {
            ServerAccessor = serverAccessor;
            ServerOptions = serverOptions;
            AuthOptions = authOptions;
        }

        [HttpGet("Miner/Get/Current")]
        public async Task<IActionResult> GetCurrentMinerAsync()
            => Ok(await ServerAccessor.GetMinerByTokenAsync(AuthOptions.Token));

        [HttpGet("Wallet/Get/Current")]
        public async Task<IActionResult> GetCurrentWalletAsync()
            => Ok(await ServerAccessor.GetWalletByTokenAsync(AuthOptions.Token));
        [HttpGet("Wallet/Get/Pool")]
        public async Task<IActionResult> GetPoolWalletAsync()
            => Ok(await ServerAccessor.GetPoolWalletAsync(AuthOptions.Token));
    }
}
