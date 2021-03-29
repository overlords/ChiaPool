using ChiaMiningManager.Models;
using ChiaMiningManager.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public Task<List<Plot>> GetPlotsAsync()
            => PlotManager.GetPlotsAsync();

        [HttpPost("DeleteId")]
        public Task<bool> DeletePlotByIdAsync([FromForm] Guid id)
            => PlotManager.DeletePlotByIdAsync(id);

        [HttpPost("DeleteName")]
        public Task<bool> DeletePlotByIdAsync([FromForm] string name)
            => PlotManager.DeletePlotByNameAsync(name);
    }
}
