using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChiaMiningManager.Controllers
{
    [Route("info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet("Status")]
        public async Task<IActionResult> GetStatusAsync()
        {


            return Ok();
        }
    }
}
