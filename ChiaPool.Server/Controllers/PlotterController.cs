using ChiaPool.Configuration.Options;
using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Plotter")]
    [ApiController]
    public class PlotterController : ControllerBase
    {
        private readonly MinerContext DbContext;
        private readonly PlotterService PlotterService;
        private readonly CustomizationOption CustomizationOptions;

        public PlotterController(MinerContext dbContext, PlotterService plotterService, CustomizationOption customizationOptions)
        {
            DbContext = dbContext;
            PlotterService = plotterService;
            CustomizationOptions = customizationOptions;
        }

        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Basic")]
        public async Task<IActionResult> CreatePlotterAsync([FromForm] string name)
        {
            if (!CustomizationOptions.AllowPlotterCreation)
            {
                return NotFound();
            }

            long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var plotter = new Plotter(name, userId);

            DbContext.Plotters.Add(plotter);
            await DbContext.SaveChangesAsync();

            return Ok(plotter);
        }

        [HttpGet("Get/Id/{id}")]
        public async Task<IActionResult> GetPlotterByIdAsync([FromRoute] long id)
        {
            var plotter = await DbContext.Plotters.FirstOrDefaultAsync(x => x.Id == id);

            if (plotter == null)
            {
                return NotFound();
            }

            var plotterInfo = PlotterService.GetPlotterInfo(plotter);
            return Ok(plotterInfo);
        }
        [HttpGet("Get/Name/{name}")]
        public async Task<IActionResult> GetPlotterByNameAsync([FromRoute] string name)
        {
            var plotter = await DbContext.Plotters.FirstOrDefaultAsync(x => x.Name == name);

            if (plotter == null)
            {
                return NotFound();
            }

            var plotterInfo = PlotterService.GetPlotterInfo(plotter);
            return Ok(plotterInfo);
        }
        [HttpGet("Get/Token/{token}")]
        public async Task<IActionResult> GetPlotterByTokenAsync([FromRoute] string token)
        {
            var plotter = await DbContext.Plotters.FirstOrDefaultAsync(x => x.Token == token);

            if (plotter == null)
            {
                return NotFound();
            }

            var plotterInfo = PlotterService.GetPlotterInfo(plotter);
            return Ok(plotterInfo);
        }

        [HttpGet("List/Id/{id}")]
        public async Task<IActionResult> GetPlotterListByOwnerIdAsync([FromRoute] long id)
        {
            var plotters = await DbContext.Plotters
                .Where(x => x.Owner.Id == id)
                .ToListAsync();

            var plotterInfos = plotters
                .Select(x => PlotterService.GetPlotterInfo(x))
                .ToList();

            return Ok(plotterInfos);
        }
        [HttpGet("List/Name/{name}")]
        public async Task<IActionResult> GetPlotterListByOwnerNameAsync([FromRoute] string name)
        {
            var plotters = await DbContext.Plotters
                .Where(x => x.Owner.Name == name)
                .ToListAsync();

            var plotterInfos = plotters
                .Select(x => PlotterService.GetPlotterInfo(x))
                .ToList();

            return Ok(plotterInfos);
        }
    }
}
