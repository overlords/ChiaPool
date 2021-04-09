using ChiaPool.Api;
using ChiaPool.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly ServerApiAccessor ServerAccessor;
        private readonly AuthOption AuthOptions;

        public ServerController(ServerApiAccessor serverAccessor, AuthOption authOptions)
        {
            ServerAccessor = serverAccessor;
            AuthOptions = authOptions;
        }

        [HttpGet("User/Get/Current")]
        public async Task<IActionResult> GetCurrentUserAsync()
            => Ok(await ServerAccessor.GetUserByNameAsync(AuthOptions.Name));
        [HttpGet("Miner/Get/Current")]
        public async Task<IActionResult> GetCurrentMinerAsync()
            => Ok(await ServerAccessor.GetMinerByTokenAsync(AuthOptions.Token));
        [HttpGet("Miner/List/Current")]
        public async Task<IActionResult> ListOwnedMinersAsync()
            => Ok(await ServerAccessor.ListMinersByOwnerNameAsync(AuthOptions.Name));

        [HttpGet("Wallet/Get/Current")]
        public async Task<IActionResult> GetCurrentWalletAsync()
            => Ok(await ServerAccessor.GetWalletByTokenAsync(AuthOptions.Name, AuthOptions.Password));
        [HttpGet("Wallet/Get/Pool")]
        public async Task<IActionResult> GetPoolWalletAsync()
            => Ok(await ServerAccessor.GetPoolWalletAsync(AuthOptions.Token));
    }
}
