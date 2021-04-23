using ChiaPool.Configuration.Options;
using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("Miner")]
    [ApiController]
    public class MinerController : ControllerBase
    {
        private readonly MinerContext DbContext;
        private readonly MinerService MinerService;
        private readonly CustomizationOption CustomizationOptions;

        public MinerController(MinerContext dbContext, MinerService minerService, CustomizationOption customizationOptions)
        {
            DbContext = dbContext;
            MinerService = minerService;
            CustomizationOptions = customizationOptions;
        }

        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Basic")]
        public async Task<IActionResult> CreateMinerAsync([FromForm] string name)
        {
            if (!CustomizationOptions.AllowMinerCreation)
            {
                return NotFound();
            }

            long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var miner = new Miner(name, userId);

            DbContext.Miners.Add(miner);
            await DbContext.SaveChangesAsync();

            return Ok(miner);
        }

        [HttpGet("Get/Id/{id}")]
        public async Task<IActionResult> GetMinerListByIdAsync([FromRoute] long id)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Id == id);

            if (miner == null)
            {
                return NotFound();
            }

            var minerInfo = MinerService.GetMinerInfo(miner);
            return Ok(minerInfo);
        }
        [HttpGet("Get/Name/{name}")]
        public async Task<IActionResult> GetMinerByNameAsync([FromRoute] string name)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Name == name);

            if (miner == null)
            {
                return NotFound();
            }

            var minerInfo = MinerService.GetMinerInfo(miner);
            return Ok(minerInfo);
        }
        [HttpGet("Get/Token/{token}")]
        public async Task<IActionResult> GetMinerByTokenAsync([FromRoute] string token)
        {
            var miner = await DbContext.Miners.FirstOrDefaultAsync(x => x.Token == token);

            if (miner == null)
            {
                return NotFound();
            }

            var minerInfo = MinerService.GetMinerInfo(miner);
            return Ok(minerInfo);
        }

        [HttpGet("List/Id/{id}")]
        public async Task<IActionResult> ListMinersByOwnerIdAsync([FromRoute] long id)
        {
            var miners = await DbContext.Miners
                .Where(x => x.Owner.Id == id)
                .ToListAsync();

            var minerInfos = miners
                .Select(x => MinerService.GetMinerInfo(x))
                .ToList();

            return Ok(minerInfos);
        }
        [HttpGet("List/Name/{name}")]
        public async Task<IActionResult> ListMinersByOwnerNameAsync([FromRoute] string name)
        {
            var miners = await DbContext.Miners
                .Where(x => x.Owner.Name == name)
                .ToListAsync();

            var minerInfos = miners
                .Select(x => MinerService.GetMinerInfo(x))
                .ToList();

            return Ok(minerInfos);
        }
    }
}
