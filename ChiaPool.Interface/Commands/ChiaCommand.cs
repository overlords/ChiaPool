using CliFx;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;

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

        protected async Task WriteAsync(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            using var colorSettings = TargetConsole.WithForegroundColor(color);
            await TargetConsole.Output.WriteAsync(message);
        }

        protected async Task WriteLineAsync(string message = "", ConsoleColor color = ConsoleColor.Gray)
        {
            using var colorSettings = TargetConsole.WithForegroundColor(color);
            await TargetConsole.Output.WriteLineAsync(message);
        }

        protected Task SuccessAsync(string message)
            => WriteAsync(message, ConsoleColor.Green);
        protected Task SuccessLineAsync(string message)
            => WriteLineAsync(message, ConsoleColor.Green);

        protected Task InfoAsync(string message)
            => WriteAsync(message, ConsoleColor.Cyan);
        protected Task InfoLineAsync(string message)
            => WriteLineAsync(message, ConsoleColor.Cyan);

        protected Task WarnAsync(string message)
            => WriteAsync(message, ConsoleColor.Yellow);
        protected Task WarnLineAsync(string message)
            => WriteLineAsync(message, ConsoleColor.Yellow);

        protected Task ErrorAsync(string message)
            => WriteAsync(message, ConsoleColor.Red);
        protected Task ErrorLineAsync(string message)
            => WriteLineAsync(message, ConsoleColor.Red);

        protected async Task TableAsync<T>(Dictionary<string, Func<T, object>> columns, T[] values, ConsoleColor headerColor = ConsoleColor.Cyan, int columnSpace = 2)
        {
            var columnNames = columns.Keys.ToArray();
            var columnSelectors = columns.Values.ToArray();

            Tuple<ConsoleColor, string>[][] contents = new Tuple<ConsoleColor, string>[values.Length + 1][];
            contents[0] = new Tuple<ConsoleColor, string>[columns.Count];
            for (int i = 0; i < columns.Count; i++)
            {
                contents[0][i] = new Tuple<ConsoleColor, string>(headerColor, columnNames[i]);
            }

            for (int i = 0; i < values.Length; i++)
            {
                contents[i + 1] = new Tuple<ConsoleColor, string>[columns.Count];

                for (int j = 0; j < columns.Count; j++)
                {
                    var value = columnSelectors[j].Invoke(values[i]);
                    var d = value.GetType();
                    if (value.GetType().Name == "ValueTuple`2" && value.GetType().GetGenericArguments().FirstOrDefault() == typeof(ConsoleColor))
                    {
                        var color = (ConsoleColor)value.GetType().GetField("Item1").GetValue(value);
                        string text = value.GetType().GetField("Item2").GetValue(value).ToString();
                        contents[i + 1][j] = new Tuple<ConsoleColor, string>(color, text);
                        continue;
                    }

                    contents[i + 1][j] = new Tuple<ConsoleColor, string>(ConsoleColor.Gray, value.ToString());
                }
            }

            int[][] spacings = new int[values.Length + 1][];
            for (int i = 0; i < values.Length + 1; i++)
            {
                spacings[i] = new int[columns.Count - 1];
            }
            for (int i = 0; i < columns.Count - 1; i++)
            {
                int maxValueLength = contents.Max(x => x[i].Item2.Length) + columnSpace;

                for (int j = 0; j < contents.Length; j++)
                {
                    spacings[j][i] = maxValueLength - contents[j][i].Item2.Length;
                }
            }


            for (int i = 0; i < values.Length + 1; i++)
            {
                for (int j = 0; j < columns.Count; j++)
                {
                    if (j != 0)
                    {
                        await WriteAsync(Space(spacings[i][j - 1]));
                    }

                    await WriteAsync(contents[i][j].Item2, contents[i][j].Item1);
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
