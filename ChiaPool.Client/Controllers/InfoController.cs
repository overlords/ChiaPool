using ChiaPool.Api;
using ChiaPool.Configuration;
using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly PlotManager PlotManager;
        private readonly ServerApiAccessor ServerAccessor;
        private readonly AuthOption AuthOptions;

        public InfoController(PlotManager plotManager, ServerApiAccessor serverAccessor, AuthOption authOptions)
        {
            PlotManager = plotManager;
            ServerAccessor = serverAccessor;
            AuthOptions = authOptions;
        }

        [HttpGet("Status")]
        public async Task<IActionResult> GetStatusAsync()
        {
            int plotCount = await PlotManager.GetPlotsCountAsync();
            var status = new ClientStatus(plotCount);

            return Ok(status);
        }

        [HttpGet("Miner")]
        public async Task<IActionResult> GetCurrentMinerAsync()
        {
            var result = await ServerAccessor.GetMinerByTokenAsync(AuthOptions.Token);
            return Ok(result);
        }
    }
}
