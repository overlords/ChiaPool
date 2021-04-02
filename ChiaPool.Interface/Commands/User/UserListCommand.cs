using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Commands.User
{
    [Command("User List")]
    public sealed class UserListCommand : ChiaCommand
    {
        private readonly ServerApiAccessor ServerAccessor;

        public UserListCommand(ServerApiAccessor serverAccessor)
        {
            ServerAccessor = serverAccessor;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var users = await ServerAccessor.ListUsersAsync();

            if (users.Count == 0)
            {
                await WarnLineAsync("No users found");
            }

            int idLenght = users.Max(x => x.Id.ToString().Length) + 2;

            await InfoLineAsync($"Id{Space(idLenght)}Name");
            foreach (var user in users)
            {
                await WriteLineAsync($"{user.Id}    {user.Name}");
            }
        }
    }
}
