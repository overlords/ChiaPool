using ChiaMiningManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChiaMiningManager.Controllers
{
    [Route("Cert")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly MinerContext DbContext;

        public CertificateController(MinerContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet("Ca")]
        public async Task<IActionResult> GetCAKeysAsync([FromHeader(Name = "Authorization")] string token)
            => !await DbContext.Miners.AnyAsync(x => x.Token == token)
                ? Unauthorized()
                : PhysicalFile("/root/ca.zip", "application/zip", "ca.zip");
    }
}
