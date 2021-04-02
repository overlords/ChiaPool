﻿using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Log Chia", Description = "Retrieves the latest chia log output")]
    public sealed class LogChiaCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;

        public LogChiaCommand(ClientApiAccessor clientAccessor)
        {
            ClientAccessor = clientAccessor;
        }

        [CommandOption("lines", 'n', Description = "Amount of log lines. Default is 100")]
        public ushort LineCount { get; set; } = 100;

        protected override async Task ExecuteAsync(IConsole console)
        {
            var lines = await ClientAccessor.GetPoolLogAsync(LineCount);

            foreach (var line in lines)
            {
                await WriteLineAsync(line);
            }
        }
    }
}
