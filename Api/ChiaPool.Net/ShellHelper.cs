using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ChiaPool
{
    public static class ShellHelper
    {
        public static Task<int> RunBashAsync(string command, ILogger logger)
        {
            var source = new TaskCompletionSource<int>();
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += (sender, args) =>
            {
                logger.LogInformation(args.Data);

            };
            process.ErrorDataReceived += (sender, args) =>
            {
                logger.LogError(args.Data);
            };
            process.Exited += (sender, args) =>
            {
                source.SetResult(process.ExitCode);
                process.Dispose();
            };

            try
            {
                process.Start();
            }
            catch 
            {
                source.SetResult(-1);
            }

            return source.Task;
        }
    }
}

