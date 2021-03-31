using ChiaMiningManager.Models;
using ChiaMiningManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaMiningManager.Controllers
{
    [Route("Info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly PlotManager PlotManager;

        public InfoController(PlotManager plotManager)
        {
            PlotManager = plotManager;
        }

        [HttpGet("Status")]
        public async Task<IActionResult> GetStatusAsync()
        {
            int plotCount = await PlotManager.GetPlotsCountAsync();

            var status = new ClientStatus();



            return Ok();
        }
    }
}
