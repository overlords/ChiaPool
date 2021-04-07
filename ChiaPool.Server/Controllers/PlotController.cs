using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Plot")]
    [ApiController]
    public class PlotController : ControllerBase
    {
        private readonly PlotterService PlotterService;
        private readonly MinerContext DbContext;

        public PlotController(PlotterService plotterService, MinerContext dbContext)
        {
            PlotterService = plotterService;
            DbContext = dbContext;
        }


        [HttpGet("Transfer/Cost/{deadlineHours}")]
        public long GetPlotCost([FromRoute] int deadlineHours = 12)
           => PlotterService.GetPlotPrice(deadlineHours);

        [HttpGet("Transfer/Buy/{deadlineHours}")]
        public async Task<IActionResult> BuyPlotTranferAsync([FromHeader(Name = "Authorization")] string token, [FromRoute] int deadlineHours = 12)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);
            if (miner == null)
            {
                return Unauthorized();
            }
            if (miner.PlotMinutes < 0)
            {
                return Forbid();
            }

            long? plotterId = PlotterService.GetSuitablePlotterId();
            if (plotterId == null)
            {
                return NotFound();
            }

            var plotter = await DbContext.Plotters.FirstOrDefaultAsync(x => x.Id == plotterId);
            var plotTransfer = await PlotterService.TryRequestPlotTransferAsync(miner.Id, plotter.Id, deadlineHours);

            plotter.PlotMinutes += plotTransfer.Cost;
            miner.PlotMinutes -= plotTransfer.Cost;
            DbContext.PlotTranfers.Add(plotTransfer);
            await DbContext.SaveChangesAsync();

            return Ok(plotTransfer);
        }
    }
}
