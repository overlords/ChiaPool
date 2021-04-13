using CliFx;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace ChiaPool.Commands
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

        protected async Task WriteAsync(string message, ConsoleColor color = default)
        {
            using var colorSettings = TargetConsole.WithForegroundColor(color);
            await TargetConsole.Output.WriteAsync(message);
        }

        protected async Task WriteLineAsync(string message = "", ConsoleColor color = default)
        {
            using var colorSettings = TargetConsole.WithForegroundColor(color);
            await TargetConsole.Output.WriteLineAsync(message);
        }

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

        protected async Task TableAsync<T>(Dictionary<string, Func<T, object>> columns, T[] values, int columnSpace = 2)
        {
            var columnNames = columns.Keys.ToArray();
            var columnSelectors = columns.Values.ToArray();

            Tuple<ConsoleColor, string>[][] contents = new Tuple<ConsoleColor, string>[columns.Count][];

            for (int i = 0; i < columns.Count; i++)
            {
                contents[i] = new Tuple<ConsoleColor, string>[values.Length];
                for (int j = 0; j < values.Length; j++)
                {
                    var value = columnSelectors[i].Invoke(values[j]);
                    contents[i][j] = value as Tuple<ConsoleColor, string> ?? new Tuple<ConsoleColor, string>(default, value.ToString());
                }
            }

            int[] headerSpaces = new int[columns.Count - 1];
            int[] valueSpaces = new int[columns.Count - 1];

            for (int i = 0; i < columns.Count - 1; i++)
            {
                headerSpaces[i] = contents[i].Max(x => x.Item2.Length) + columnSpace - columnNames[i].Length;
            }
            for (int i = 0; i < columns.Count - 1; i++)
            {
                if (headerSpaces[i] > 0)
                {
                    valueSpaces[i] = columnSpace;
                    continue;
                }

                valueSpaces[i] = columnSpace - headerSpaces[i] + 1;
                headerSpaces[i] = 1;
            }

            for (int i = 0; i < contents.Length; i++)
            {
                for (int j = 0; j < contents[i].Length; j++)
                {
                    if (j != 0)
                    {
                        await WriteAsync(Space(i == 0 ? headerSpaces[j] : valueSpaces[j]));
                    }

                    await (i == 0
                        ? InfoAsync(columnNames[j])
                        : WriteAsync(contents[i][j].Item2, contents[i][j].Item1));
                }
                await WriteLineAsync();
            }
        }

        protected async Task<string> ReadLineAsync()
            => await TargetConsole.Input.ReadLineAsync();
        protected async Task<char> ReadKeyAsync()
        {
            Memory<char> buffer = new char[1];
            await TargetConsole.Input.ReadAsync(buffer);
            return buffer.Span[0];
        }

        protected string Space(int length)
            => new string(' ', Math.Max(length, 1));
    }
}
