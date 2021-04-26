using ChiaPool.Configuration.Options;
using ChiaPool.Models;
using ChiaPool.Services;
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
        private readonly HashingService HashingService;
        private readonly UserService UserService;
        private readonly CustomizationOption CustomizationOptions;

        public UserController(MinerContext dbContext, HashingService hashingService, CustomizationOption customizationOptions, UserService userService)
        {
            DbContext = dbContext;
            HashingService = hashingService;
            CustomizationOptions = customizationOptions;
            UserService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromForm] string name, [FromForm] string password)
        {
            if (!CustomizationOptions.AllowUserRegistration)
            {
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                return UnprocessableEntity();
            }
            if (await DbContext.Users.AnyAsync(x => x.Name.ToUpper() == name.ToUpper()))
            {
                return Conflict("Username already taken!");
            }

            string passwordHash = HashingService.HashString(password);
            var user = new User(name, passwordHash);
            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync();

            var userInfo = UserService.GetUserInfo(user);

            return Ok(userInfo);
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await DbContext.Users.ToListAsync();
            var userInfos = users.Select(x => UserService.GetUserInfo(x));
            return Ok(userInfos);
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

            var userInfo = UserService.GetUserInfo(user);
            return Ok(userInfo);
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

            var userInfo = UserService.GetUserInfo(user);
            return Ok(userInfo);
        }
    }
}
