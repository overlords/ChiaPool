using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusService StatusService;

        public StatusController(StatusService statusService)
        {
            StatusService = statusService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetStatusAsync()
        {
            var status = await StatusService.GetStatusAsync();
            return Ok(status);
        }
    }
}
