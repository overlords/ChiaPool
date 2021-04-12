using ChiaPool.Api;
using ChiaPool.Configuration;
using ChiaPool.Services;
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
        private readonly ConnectionManager ConnectionManager;

        public ServerController(ServerApiAccessor serverAccessor, AuthOption authOptions, ConnectionManager connectionManager)
        {
            ServerAccessor = serverAccessor;
            AuthOptions = authOptions;
            ConnectionManager = connectionManager;
        }

        [HttpGet("User/Get/Current")]
        public async Task<IActionResult> GetCurrentUserAsync()
            => Ok(await ServerAccessor.GetUserByIdAsync(ConnectionManager.GetCurrentUserId()));
        [HttpGet("Miner/Get/Current")]
        public async Task<IActionResult> GetCurrentMinerAsync()
            => Ok(await ServerAccessor.GetMinerByTokenAsync(AuthOptions.Token));
        [HttpGet("Miner/List/Current")]
        public async Task<IActionResult> ListOwnedMinersAsync()
            => Ok(await ServerAccessor.ListMinersByOwnerIdAsync(ConnectionManager.GetCurrentUserId()));
        [HttpGet("Plotter/List/Current")]
        public async Task<IActionResult> ListOwnedPlottersAsync()
            => Ok(await ServerAccessor.ListPlottersByOwnerIdAsync(ConnectionManager.GetCurrentUserId()));

        [HttpGet("Wallet/Get/Current")]
        public async Task<IActionResult> GetCurrentWalletAsync()
            => Ok(await ServerAccessor.GetWalletByOwnerIdAsync(ConnectionManager.GetCurrentUserId()));
    }
}
