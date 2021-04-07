using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Log")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private const string ChiaLogFile = "/root/.chia/mainnet/log/debug.log";
        private const string PoolLogFile = "/root/.chia/mainnet/log/pool.log";

        [HttpGet("Chia/{lineCount}")]
        public async Task<IActionResult> GetChiaLogAsync([FromRoute] ushort lineCount)
        {
            if (!System.IO.File.Exists(ChiaLogFile))
            {
                return Ok(Array.Empty<string>());
            }

            var logLines = await System.IO.File.ReadAllLinesAsync(ChiaLogFile);
            return Ok(logLines.TakeLast(lineCount));
        }

        [HttpGet("Pool/{lineCount}")]
        public async Task<IActionResult> GetPoolLogAsync([FromRoute] ushort lineCount)
        {
            if (!System.IO.File.Exists(PoolLogFile))
            {
                return Ok(Array.Empty<string>());
            }

            var logLines = await System.IO.File.ReadAllLinesAsync(PoolLogFile);
            return Ok(logLines.TakeLast(lineCount));
        }
    }
}
