using ChiaPool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Plot")]
    [ApiController]
    public class PlotController : ControllerBase
    {
        private readonly PlotContext DbContext;

        public PlotController(PlotContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet("Download/{secret}")]
        public async Task<IActionResult> DownloadPlotAsync([FromRoute] string secret)
        {
            var storedPlot = await DbContext.StoredPlots.FirstOrDefaultAsync(x => x.Secret == secret);

            return storedPlot.Available || storedPlot.Deleted
                ? NotFound()
                : PhysicalFile(storedPlot.Path, "Chia/Plot", true);
        }
    }
}
