using ChiaPool.Extensions;
using ChiaPool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MinerContext DbContext;

        public UserController(MinerContext dbContext)
        {
            DbContext = dbContext;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await DbContext.Users.ToListAsync();
            var publicUsers = users.Select(x => x.WithoutSecrets()).ToList();
            return Ok(publicUsers);
        }

        [HttpGet("Get/Name/{name}")]
        public async Task<IActionResult> GetUserByNameAsync([FromRoute] string name)
        {
            var user = await DbContext.Users
                .Include(x => x.Miners)
                .FirstOrDefaultAsync(x => x.Name == name);

            if (user == null)
            {
                return NotFound();
            }

            user.MinerState = MinerState.FromMiners(user.Miners);
            return Ok(user.WithoutSecrets());
        }
        [HttpGet("Get/Id/{id}")]
        public async Task<IActionResult> GetUserByNameAsync([FromRoute] long id)
        {
            var user = await DbContext.Users
                .Include(x => x.Miners)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.MinerState = MinerState.FromMiners(user.Miners);
            return Ok(user.WithoutSecrets());
        }
    }
}
