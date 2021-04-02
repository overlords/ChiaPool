using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Commands.User
{
    [Command("User Show", Description = "Shows information about a user")]
    public sealed class UserShowCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public UserShowCommand(ClientApiAccessor clientAccessor, ServerApiAccessor serverAccessor)
        {
            ClientAccessor = clientAccessor;
            ServerAccessor = serverAccessor;
        }

        [CommandOption("name", 'n', Description = "The name of the user")]
        public string Name { get; set; }

        [CommandOption("id", 'i', Description = "The id of the user")]
        public long Id { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            if (Name != default && Id != default)
            {
                await ErrorAsync("Please specify either name or id, not both");
                return;
            }

            var user = Name != default
                ? await ServerAccessor.GetUserByNameAsync(Name)
                : Id != default
                ? await ServerAccessor.GetUserByIdAsync(Id)
                : await ClientAccessor.GetCurrentUserAync();

            if (user == null)
            {
                await WarnLineAsync("User not found");
                return;
            }

            var miners = Name != default
                ? await ServerAccessor.ListMinersByNameAsync(Name)
                : Id != default
                ? await ServerAccessor.ListMinersByIdAsync(Id)
                : await ClientAccessor.ListOwnedMinersAsync();


            await InfoLineAsync($" [ {user.Name}   |   User Info ] ");

            await InfoLineAsync($"[ID]             |  {user.Id}");
            await InfoLineAsync($"[Miner Count]    |  {user.MinerState.MinerCount}");
            await InfoLineAsync($"[Plot Count]     |  {user.MinerState.PlotCount}");
            await InfoLineAsync($"[Plot Minutes]   |  {user.MinerState.PlotMinutes}");

            await WriteLineAsync();

            await InfoLineAsync($" [ {user.Name}   |   Miner Info ] ");

            if (miners.Count == 0)
            {
                await WarnLineAsync("None");
                return;
            }

            int idLength = miners.Max(x => x.Id.ToString().Length) + 2;
            int nameLength = miners.Max(x => x.Name.Length);

            await InfoLineAsync($"Id{Space(idLength)}Name{Space(nameLength)}PM");

            foreach (var miner in miners)
            {
                await WriteLineAsync($"{miner.Id}    {miner.Name}    {miner.PlotMinutes}");
            }
        }
    }
}
