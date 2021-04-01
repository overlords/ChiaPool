using ChiaPool.Api;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Commands.Miner
{
    [Command("Miner Info", Description = "Retrives information about a specific miner. Defaults to you")]
    public class MinerInfoCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public MinerInfoCommand(ClientApiAccessor clientAccessor, ServerApiAccessor serverAccessor)
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

            await InfoAsync($"Miner ID      |   {miner.Id}");
            await InfoAsync($"Miner Name    |   {miner.Name}");

        }
    }
}
