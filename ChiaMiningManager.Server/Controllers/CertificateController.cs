using ChiaMiningManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO.Compression;
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
        {
            if (!await DbContext.Miners.AnyAsync(x => x.Token == token))
            {
                return Unauthorized();
            }

            var file = ZipFile.CreateFromDirectory()

            return Ok();
        }
    }
}
