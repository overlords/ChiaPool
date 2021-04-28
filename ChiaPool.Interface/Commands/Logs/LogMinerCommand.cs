using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Log Miner", Description = "Retrieves the latest pool log output")]
    public class LogMinerCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;

        public LogMinerCommand(MinerApiAccessor minerAccessor)
        {
            MinerAccessor = minerAccessor;
        }

        [CommandOption("lines", 'n', Description = "Amount of log lines. Default is 100")]
        public ushort LineCount { get; set; } = 100;

        protected override async Task ExecuteAsync(IConsole console)
        {
            var lines = await MinerAccessor.GetPoolLogAsync(LineCount);

            if (lines.Length == 0)
            {
                await InfoAsync("--- No entries ---");
                return;
            }

            await InfoAsync($"Showing the last {lines.Length} lines:");
            await WriteLineAsync();

            foreach (var line in lines)
            {
                await WriteLineAsync(line);
            }
        }
    }
}
