using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly PlotManager PlotManager;

        public StatusController(PlotManager plotManager)
        {
            PlotManager = plotManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetStatusAsync()
        {
            int plotCount = await PlotManager.GetPlotCountAsync();
            var status = new MinerStatus(plotCount);

            return Ok(status);
        }
    }
}
