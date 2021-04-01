using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Plots")]
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

        [HttpPost("DeleteId")]
        public Task<bool> DeletePlotByPubKeyAsync([FromForm] string pubKey)
            => PlotManager.DeletePlotByPubKeyAsync(pubKey);

        [HttpPost("DeleteName")]
        public Task<bool> DeletePlotByFileNameAsync([FromForm] string fileName)
            => PlotManager.DeletePlotByFileNameAsync(fileName);
    }
}
