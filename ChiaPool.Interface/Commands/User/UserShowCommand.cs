using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("User Show", Description = "Shows information about a user")]
    public sealed class UserShowCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public UserShowCommand(MinerApiAccessor minerAccessor, ServerApiAccessor serverAccessor)
        {
            MinerAccessor = minerAccessor;
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
                : await MinerAccessor.GetCurrentUserAync();

            if (user == null)
            {
                await WarnLineAsync("User not found");
                return;
            }

            var miners = Name != default
                ? await ServerAccessor.ListMinersByOwnerNameAsync(Name)
                : Id != default
                ? await ServerAccessor.ListMinersByOwnerIdAsync(Id)
                : await MinerAccessor.ListOwnedMinersAsync();

            var plotters = Name != default
                ? await ServerAccessor.ListPlottersByOwnerNameAsync(Name)
                : Id != default
                ? await ServerAccessor.ListPlottersByOwnerIdAsync(Id)
                : await MinerAccessor.ListOwnedPlottersAsync();

            await InfoLineAsync($"[ {user.Name}   |   User Info ] ");

            await InfoLineAsync($"[ID]             |  {user.Id}");
            await InfoLineAsync($"[Miner Count]    |  {miners.Count}");
            await InfoLineAsync($"[Miner PM]       |  {miners.Sum(x => x.PlotMinutes)}");
            await InfoLineAsync($"[Plotter Count]  |  {plotters.Count}");
            await InfoLineAsync($"[Plotter PM]     |  {plotters.Sum(x => x.PlotMinutes)}");

            await WriteLineAsync();
            await InfoLineAsync($"[ {user.Name}   |   Miners ] ");

            if (miners.Count == 0)
            {
                await WarnLineAsync("--- No Entries ---");
            }
            else
            {
                int idLength = miners.Max(x => x.Id.ToString().Length) + 2;
                int nameLength = miners.Max(x => x.Name.Length);
                int pmLength = miners.Max(x => x.PlotMinutes.ToString().Length + 2);

                await InfoLineAsync($"Id{Space(idLength)}Name{Space(nameLength)}PM{Space(pmLength)}Status");

                foreach (var miner in miners)
                {
                    await WriteAsync($"{miner.Id}    {miner.Name}    {miner.PlotMinutes}    ");
                    await (miner.Online
                        ? SuccessLineAsync("Online")
                        : ErrorLineAsync("Offline"));
                }
            }

            await WriteLineAsync();
            await InfoLineAsync($"[ {user.Name}   |   Plotters ]");

            if (plotters.Count == 0)
            {
                await WarnLineAsync("--- No Entries ---");
            }
            else
            {
                int idLength = plotters.Max(x => x.Id.ToString().Length) + 2;
                int nameLength = plotters.Max(x => x.Name.Length);
                int pmLength = plotters.Max(x => x.PlotMinutes.ToString().Length + 2);

                await InfoLineAsync($"Id{Space(idLength)}Name{Space(nameLength)}PM{Space(pmLength)}Status");

                foreach (var plotter in plotters)
                {
                    await WriteAsync($"{plotter.Id}    {plotter.Name}    {plotter.PlotMinutes}    ");
                    await (plotter.Online
                        ? SuccessLineAsync("Online")
                        : ErrorLineAsync("Offline"));
                }
            }
        }
    }
}
