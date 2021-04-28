using Chia.NET.Models;
using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Plot")]
    [ApiController]
    public class PlotController : ControllerBase
    {
        private readonly PlotService PlotManager;
        private readonly StatusService StatusService;

        public PlotController(PlotService plotManager, StatusService statusService)
        {
            PlotManager = plotManager;
            StatusService = statusService;
        }

        [HttpGet("List")]
        public Task<Plot[]> GetPlotsAsync()
            => PlotManager.GetPlotsAsync();

        [HttpPost("Reload")]
        public async Task ReloadPlotsAsync()
        {
            await PlotManager.ReloadPlotsAsync();
            await StatusService.RefreshStatusAsync();
        }

        [HttpPost("DeleteKey")]
        public Task<bool> DeletePlotByPubKeyAsync([FromForm] string pubKey)
            => PlotManager.DeletePlotByPubKeyAsync(pubKey);

        [HttpPost("DeleteFile")]
        public Task<bool> DeletePlotByFileNameAsync([FromForm] string fileName)
            => PlotManager.DeletePlotByFileNameAsync(fileName);

        [HttpPost("Generate")]
        public IActionResult StartPlotGeneration([FromBody] PlottingConfiguration configuration)
        {
            _ = Task.Run(() => PlotManager.GeneratePlotAsync(configuration));
            return Ok();
        }
    }
}
