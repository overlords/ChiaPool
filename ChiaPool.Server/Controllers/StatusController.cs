using ChiaPool.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetStatusAsync()
        {
            var status = new ServerStatus();
            return Ok(status);
        }
    }
}
