﻿using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plotter Show", Description = "Retrieves information about a specific plotter")]
    public sealed class PlotterShowCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        [CommandOption("id", 'i', Description = "The Id of the plotter")]
        public long? Id { get; set; }

        [CommandOption("name", 'n', Description = "The name of the plotter")]
        public string Name { get; set; }

        public PlotterShowCommand(MinerApiAccessor minerAccessor, ServerApiAccessor serverAcccessor)
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
            if (Name == default && Id == default)
            {
                await ErrorAsync("Please specify either name or id");
                return;
            }


            var plotter = Name != default
                ? await ServerAccessor.GetPlotterByNameAsync(Name)
                : await ServerAccessor.GetPlotterByIdAsync(Id.Value);

            await InfoLineAsync($"[ID]         |   {plotter.Id}");
            await InfoLineAsync($"[Name]       |   {plotter.Name}");
            await InfoLineAsync($"[Earnings]   |   {plotter.Earnings}");
            await InfoAsync($"[Status]     |   ");

            if (!plotter.Online)
            {
                await ErrorLineAsync("Offline");
                return;
            }

            await SuccessLineAsync("Online");
            await InfoLineAsync($"[Capacity]   |   {plotter.Capacity}");
            await InfoLineAsync($"[Plots]      |   {plotter.PlotsAvailable}");
        }
    }
}
