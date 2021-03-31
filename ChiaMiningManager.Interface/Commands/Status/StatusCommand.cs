﻿using ChiaMiningManager.Api;
using ChiaMiningManager.Extensions;
using ChiaMiningManager.Models;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands.Status
{
    [Command("Status", Description = "Get miner status")]
    public class StatusCommand : ICommand
    {
        private readonly ServerApiAccessor ServerAccessor;
        private readonly ClientApiAccessor ClientAccessor;

        public StatusCommand(ServerApiAccessor serverAccessor, ClientApiAccessor clientAccessor)
        {
            ServerAccessor = serverAccessor;
            ClientAccessor = clientAccessor;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var clientStatusTask = ClientAccessor.GetStatusAsync().Try();
            var serverStatusTask = ServerAccessor.GetStatusAsync().Try();

            ClientStatus clientStatus = await clientStatusTask;
            ServerStatus serverStatus = await serverStatusTask;

            console.WithForegroundColor(ConsoleColor.Cyan);
            await console.Output.WriteAsync($"Client ");

            if (clientStatus == null)
            {
                console.WithForegroundColor(ConsoleColor.DarkRed);
                await console.Output.WriteLineAsync("Offline");
            }
            else
            {
                await console.Output.WriteLineAsync("Online");
            }

            console.WithForegroundColor(ConsoleColor.Cyan);
            await console.Output.WriteAsync($"Server ");

            if (serverStatus == null)
            {
                console.WithForegroundColor(ConsoleColor.DarkRed);
                await console.Output.WriteLineAsync("Offline");
            }
            else
            {
                await console.Output.WriteLineAsync("Online");
            }

            console.ResetColor();
        }
    }
}