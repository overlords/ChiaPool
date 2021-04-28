using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
        public Task<long> GetPlotCost([FromRoute] int deadlineHours = 12)
           => PlotterService.GetPlotPriceAsync(deadlineHours);

        [HttpGet("Transfer/Buy/{deadlineHours}")]
        [Authorize(AuthenticationSchemes = "Miner")]
        public async Task<IActionResult> BuyPlotTranferAsync([FromRoute] int deadlineHours = 12)
        {
            long minerId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var miner = await DbContext.Miners
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == minerId);
            var customer = miner.Owner;

            if (customer.PlotMinutes < 0)
            {
                return Conflict();
            }

            long? plotterId = await PlotterService.GetSuitablePlotterIdAsync();
            if (plotterId == null)
            {
                return NotFound();
            }

            var plotter = await DbContext.Plotters
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == plotterId);
            var seller = plotter.Owner;

            var plotTransfer = await PlotterService.TryRequestPlotTransferAsync(miner.Id, plotter.Id, deadlineHours);

            plotter.Earnings += plotTransfer.Cost;
            seller.PlotMinutes += plotTransfer.Cost;
            customer.PlotMinutes -= plotTransfer.Cost;

            DbContext.PlotTranfers.Add(plotTransfer);
            await DbContext.SaveChangesConcurrentAsync();

            return Ok(plotTransfer);
        }
    }
}
