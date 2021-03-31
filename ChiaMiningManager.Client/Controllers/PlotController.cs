using Chia.NET.Clients;
using Chia.NET.Models;
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
        private readonly HarvesterClient HarvesterClient;

        public PlotController(HarvesterClient harvesterClient)
        {
            HarvesterClient = harvesterClient;
        }

        [HttpGet("List")]
        public Task<Plot[]> GetPlotsAsync()
            => HarvesterClient.GetPlotsAsync();

        //[HttpPost("DeleteId")]
        //public Task<bool> DeletePlotByIdAsync([FromForm] Guid id)
        //    => PlotManager.DeletePlotByIdAsync(id);

        //[HttpPost("DeleteName")]
        //public Task<bool> DeletePlotByIdAsync([FromForm] string name)
        //    => PlotManager.DeletePlotByNameAsync(name);
    }
}
