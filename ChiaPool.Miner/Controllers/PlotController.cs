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
        private readonly PlotManager PlotManager;

        public PlotController(PlotManager plotManager)
        {
            PlotManager = plotManager;
        }

        [HttpGet("List")]
        public Task<PlotInfo[]> GetPlotsAsync()
            => PlotManager.GetPlotsAsync();

        [HttpPost("Reload")]
        public Task ReloadPlotsAsync()
            => PlotManager.ReloadPlotsAsync();

        [HttpPost("DeleteKey")]
        public Task<bool> DeletePlotByPubKeyAsync([FromForm] string pubKey)
            => PlotManager.DeletePlotByPubKeyAsync(pubKey);

        [HttpPost("DeleteFile")]
        public Task<bool> DeletePlotByFileNameAsync([FromForm] string fileName)
            => PlotManager.DeletePlotByFileNameAsync(fileName);

        [HttpPost("Generate")]
        public async Task<IActionResult> StartPlotGenerationAsync([FromForm] PlottingConfiguration configuration)
        {
            await PlotManager.GeneratePlotAsync(configuration);
            return Ok();
        }
    }
}
