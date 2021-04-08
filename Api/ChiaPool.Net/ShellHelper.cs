using ChiaPool.Models;
using Microsoft.Extensions.Logging;
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

        public static async Task<int> RunPlotGenerationAsync(PlottingConfiguration config, ILogger logger)
        {
            var sw = new Stopwatch();
            sw.Start();

            string command = $"cd /root/chia-blockchain && . ./activate && chia plots create -k {config.Size} -d {config.Path} -t {config.CachePath} -u {config.BucketCount} -b {config.BufferSize}";
            logger.LogInformation(command);
            int exitCode = await RunBashAsync(command, logger);

            sw.Stop();

            if (exitCode == 0)
            {
                logger.LogInformation($"Finished plotting process after {sw.Elapsed.TotalMinutes} minutes");
            }
            else
            {
                logger.LogError($"Plotting failed after {sw.Elapsed.TotalMinutes} minutes");
            }

            return exitCode;
        }
    }
}

