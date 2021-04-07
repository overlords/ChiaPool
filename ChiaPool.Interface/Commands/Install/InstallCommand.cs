using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Install")]
    public sealed class InstallCommand : ChiaCommand
    {
        protected override async Task ExecuteAsync(IConsole console)
        {
            await InfoLineAsync("Welcome to the installation assistent!");
            await WriteLineAsync();
            await InfoLineAsync("Before continuing make sure to install docker for windows and verify that it is working!");
            await WriteLineAsync("");
            await InfoLineAsync("Enter to continue...");

            await console.Input.ReadLineAsync();

            await InfoLineAsync("Let's configure your credentials");

            string username = await GetAnswerAsync(console, "What is your username?");
            string password = await GetAnswerAsync(console, "What is your password?");
            string token = await GetAnswerAsync(console, "What is your miner token?");

            await InfoLineAsync("Done! Press enter to continue to the next segment...");
            await console.Output.FlushAsync();
            await console.Output.DisposeAsync();

            await InfoLineAsync("Let's continue with the server settings");

            await InfoLineAsync("What is the pool address? Should be a hostname / ip without a protocol prefix");
            string host = await console.Input.ReadLineAsync();
            await WriteLineAsync();

            await InfoLineAsync("");
        }

        private async Task<string> GetAnswerAsync(IConsole console, string question)
        {
            await InfoLineAsync(question);
            var answer = await console.Input.ReadLineAsync();
            await WriteLineAsync();
            return answer;
        }
    }
}
