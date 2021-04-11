using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Log Chia", Description = "Retrieves the latest chia log output")]
    public sealed class LogChiaCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;

        public LogChiaCommand(MinerApiAccessor minerAccessor)
        {
            MinerAccessor = minerAccessor;
        }

        [CommandOption("lines", 'n', Description = "Amount of log lines. Default is 100")]
        public ushort LineCount { get; set; } = 100;

        protected override async Task ExecuteAsync(IConsole console)
        {
            var lines = await MinerAccessor.GetChiaLogAsync(LineCount);

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
