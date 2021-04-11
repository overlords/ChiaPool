﻿using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Miner Show", Description = "Retrieves information about a specific miner. Defaults to you")]
    public class MinerShowCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        [CommandOption("id", 'i', Description = "The Id of the miner")]
        public long Id { get; set; }

        [CommandOption("name", 'n', Description = "The name of the miner")]
        public string Name { get; set; }

        public MinerShowCommand(MinerApiAccessor minerAccessor, ServerApiAccessor serverAcccessor)
        {
            MinerAccessor = minerAccessor;
            ServerAccessor = serverAcccessor;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            if (Name != default && Id != default)
            {
                await ErrorAsync("Please specify either name or id, not both");
                return;
            }

            var miner = Name != default
                ? await ServerAccessor.GetMinerByNameAsync(Name)
                : Id != default
                ? await ServerAccessor.GetMinerByIdAsync(Id)
                : await MinerAccessor.GetCurrentMinerAsync();

            await InfoLineAsync($"[ID]      |   {miner.Id}");
            await InfoLineAsync($"[Name]    |   {miner.Name}");
            await InfoLineAsync($"[PM]      |   {miner.PlotMinutes}");
            await InfoLineAsync($"[Online]  |   {miner.Online}");
        }
    }
}
