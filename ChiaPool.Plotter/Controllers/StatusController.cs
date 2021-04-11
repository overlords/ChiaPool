using ChiaPool.Services;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetCurrentStatus()
            => Ok(StatusService.GetCurrentStatus());
    }
}
