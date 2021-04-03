using ChiaPool.Api;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Log Pool", Description = "Retrieves the latest pool log output")]
    public class LogPoolCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;

        public LogPoolCommand(ClientApiAccessor clientAccessor)
        {
            ClientAccessor = clientAccessor;
        }

        [CommandOption("lines", 'n', Description = "Amount of log lines. Default is 100")]
        public ushort LineCount { get; set; } = 100;

        protected override async Task ExecuteAsync(IConsole console)
        {
            var lines = await ClientAccessor.GetPoolLogAsync(LineCount);

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
