using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ChiaPool.Controllers
{
    [Route("Cert")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        [HttpGet("Ca")]
        [Authorize(AuthenticationSchemes = "Miner")]
        public IActionResult GetCAKeys()
            => PhysicalFile("/root/ca.zip", "application/zip", "ca.zip");

        [HttpGet("Keys")]
        [Authorize]
        public IActionResult GetKeys()
        {
            string farmerKeys = Environment.GetEnvironmentVariable("farmer_keys");
            return Ok(farmerKeys);
        }
    }
}
