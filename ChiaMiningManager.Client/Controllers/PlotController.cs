using ChiaMiningManager.Models;
using ChiaMiningManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaMiningManager.Controllers
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

        //[HttpPost("DeleteId")]
        //public Task<bool> DeletePlotByIdAsync([FromForm] Guid id)
        //    => PlotManager.DeletePlotByIdAsync(id);

        //[HttpPost("DeleteName")]
        //public Task<bool> DeletePlotByIdAsync([FromForm] string name)
        //    => PlotManager.DeletePlotByNameAsync(name);
    }
}
