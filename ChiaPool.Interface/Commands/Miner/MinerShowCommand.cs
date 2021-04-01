using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Commands.Miner
{
    [Command("Miner Show", Description = "Retrieves information about a specific miner. Defaults to you")]
    public class MinerShowCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public MinerShowCommand(ClientApiAccessor clientAccessor, ServerApiAccessor serverAccessor)
        {
            ClientAccessor = clientAccessor;
            ServerAccessor = serverAccessor;
        }

        [CommandOption("name", 'n', Description = "The name of the miner")]
        public string Name { get; set; }

        [CommandOption("id", 'i', Description = "The id of the miner")]
        public Guid Id { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            if (Name != default && Id != default)
            {
                await ErrorAsync("Please specify either Name or Id, not both");
            }

            var miner = Name != default
                ? await ServerAccessor.GetMinerByNameAsync(Name)
                : Id != default
                ? await ServerAccessor.GetMinerByIdAsync(Id)
                : await ClientAccessor.GetCurrentMinerAsync();

            await InfoAsync($"[ID]      |   {miner.Id}");
            await InfoAsync($"[Name]    |   {miner.Name}");
            await InfoAsync($"[PM]      |   {miner.PlotMinutes}");
        }
    }
}
