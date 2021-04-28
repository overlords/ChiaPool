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

        public PlotController(PlotService plotManager)
        {
            PlotManager = plotManager;
        }

        [HttpGet("List")]
        public Task<Plot[]> GetPlotsAsync()
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
        public IActionResult StartPlotGeneration([FromBody] PlottingConfiguration configuration)
        {
            _ = Task.Run(() => PlotManager.GeneratePlotAsync(configuration));
            return Ok();
        }
    }
}
