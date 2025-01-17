﻿using ChiaPool.Api;
using ChiaPool.Extensions;
using ChiaPool.Models;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Miner Create")]
    public sealed class MinerCreateCommand : ChiaCommand
    {
        private readonly ServerApiAccessor ServerAccessor;

        public MinerCreateCommand(ServerApiAccessor serverAccessor)
        {
            ServerAccessor = serverAccessor;
        }

        [CommandParameter(0, Name = "Name", Description = "Name of the miner to create")]
        public string Name { get; set; }

        [CommandParameter(1, Name = "Username", Description = "The name of the user used for authentication")]
        public string Username { get; set; }

        [CommandParameter(2, Name = "Password", Description = "The password of the user used for authentication")]
        public string Password { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var creationTask = ServerAccessor.CreateMinerAsync(Name, Username, Password);
            var minerResult = await creationTask.Try();

            if (minerResult == null)
            {
                var response = (creationTask.Exception.InnerException as HttpRequestException)?.StatusCode;
                if (response == null || !response.HasValue)
                {
                    throw creationTask.Exception.InnerException;
                }
                var responseCode = response.Value;

                if (responseCode == HttpStatusCode.NotFound)
                {
                    await ErrorLineAsync("This pool does not support miner creation!");
                }
                else if (responseCode == HttpStatusCode.Unauthorized)
                {
                    await ErrorLineAsync("Your credentials are wrong!");
                }
                else
                {
                    throw creationTask.Exception.InnerException;
                }

                return;
            }

            await SuccessLineAsync("Successfully created miner!");
            await TableAsync(new Dictionary<string, Func<CreateMinerResult, object>>()
            {
                ["Id"] = x => x.Info.Id,
                ["Name"] = x => x.Info.Name,
                ["Token"] = x => x.Token,
            }, new[] { minerResult });
        }
    }
}
