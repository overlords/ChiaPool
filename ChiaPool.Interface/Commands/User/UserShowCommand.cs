using ChiaPool.Api;
using ChiaPool.Models;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            await InfoLineAsync($"[ID]               |  {user.Id}");
            await InfoLineAsync($"[Plot Minutes]     |  {user.PlotMinutes}");
            await InfoLineAsync($"[Miner Count]      |  {miners.Count}");
            await InfoLineAsync($"[Mining Profit]    |  {miners.Sum(x => x.Earnings)}");
            await InfoLineAsync($"[Plotter Count]    |  {plotters.Count}");
            await InfoLineAsync($"[Plotting Profit]  |  {plotters.Sum(x => x.Earnings)}");

            await WriteLineAsync();
            await InfoLineAsync($"[ {user.Name}   |   Miners ] ");

            if (miners.Count == 0)
            {
                await WarnLineAsync("--- No Entries ---");
            }
            else
            {
                var columns = new Dictionary<string, Func<MinerInfo, object>>()
                {
                    ["Id"] = x => x.Id,
                    ["Name"] = x => x.Name,
                    ["Earnings"] = x => x.Earnings,
                    ["Status"] = x => (x.Online ? ConsoleColor.Green : ConsoleColor.Red, x.Online ? "Online" : "Offline"),
                    ["Plots"] = x => x.Online ? x.PlotCount : "",
                };

                await TableAsync(columns, miners.ToArray());
            }


            await WriteLineAsync();
            await InfoLineAsync($"[ {user.Name}   |   Plotters ]");

            if (plotters.Count == 0)
            {
                await WarnLineAsync("--- No Entries ---");
            }
            else
            {
                var columns = new Dictionary<string, Func<PlotterInfo, object>>()
                {
                    ["Id"] = x => x.Id,
                    ["Name"] = x => x.Name,
                    ["Earnings"] = x => x.Earnings,
                    ["Status"] = x => (x.Online ? ConsoleColor.Green : ConsoleColor.Red, x.Online ? "Online" : "Offline"),
                    ["Capacity"] = x => x.Online ? x.Capacity : "",
                    ["Available Plots"] = x => x.Online ? x.PlotsAvailable : "",
                };

                await TableAsync(columns, plotters.ToArray());
            }
        }
    }
}
