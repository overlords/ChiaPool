using CliFx;
using CliFx.Infrastructure;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands
{
    public abstract class ChiaCommand : ICommand
    {
        private IConsole TargetConsole;

        async ValueTask ICommand.ExecuteAsync(IConsole console)
        {
            TargetConsole = console;
            try
            {
                await ExecuteAsync(console);
            }
            catch (HttpRequestException)
            {
                await ErrorLineAsync("There was a connection error!");
                await ErrorLineAsync("Run the Status command to see more");
            }
        }

        protected abstract Task ExecuteAsync(IConsole console);

        protected Task WriteAsync(string message)
            => TargetConsole.Output.WriteAsync(message);
        protected Task WriteLineAsync(string message)
            => TargetConsole.Output.WriteLineAsync(message);

        protected async Task SuccessAsync(string message)
        {
            using var colorSetting = TargetConsole.WithForegroundColor(ConsoleColor.Green);
            await WriteAsync(message);
        }
        protected async Task SuccessLineAsync(string message)
        {
            using var colorSetting = TargetConsole.WithForegroundColor(ConsoleColor.Green);
            await WriteLineAsync(message);
        }

        protected async Task InfoAsync(string message)
        {
            using var colorSetting = TargetConsole.WithForegroundColor(ConsoleColor.Cyan);
            await WriteAsync(message);
        }
        protected async Task InfoLineAsync(string message)
        {
            using var colorSetting = TargetConsole.WithForegroundColor(ConsoleColor.Cyan);
            await WriteLineAsync(message);
        }

        protected async Task WarnAsync(string message)
        {
            using var colorSetting = TargetConsole.WithForegroundColor(ConsoleColor.Yellow);
            await WriteAsync(message);
        }        
        protected async Task WarnLineAsync(string message)
        {
            using var colorSetting = TargetConsole.WithForegroundColor(ConsoleColor.Yellow);
            await WriteLineAsync(message);
        }

        protected async Task ErrorAsync(string message)
        {
            using var colorSetting = TargetConsole.WithForegroundColor(ConsoleColor.Red);
            await WriteAsync(message);
        }
        protected async Task ErrorLineAsync(string message)
        {
            using var colorSetting = TargetConsole.WithForegroundColor(ConsoleColor.Red);
            await WriteLineAsync(message);
        }

        protected string Space(int length)
            => new string(' ', length);
    }
}
