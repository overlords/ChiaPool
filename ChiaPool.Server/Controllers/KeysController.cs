using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ChiaPool.Controllers
{
    [Route("Keys")]
    [ApiController]
    public class KeysController : ControllerBase
    {
        [HttpGet("Plotting")]
        [Authorize(AuthenticationSchemes = "Miner, Plotter")]
        public IActionResult GetKeys()
        {
            string farmerKeys = Environment.GetEnvironmentVariable("plotting_keys");
            return Ok(farmerKeys);
        }
    }
}
