using ChiaPool.Api;
using ChiaPool.Extensions;
using ChiaPool.Models;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plotter Create")]
    public sealed class PlotterCreateCommand : ChiaCommand
    {
        private readonly ServerApiAccessor ServerAccessor;

        public PlotterCreateCommand(ServerApiAccessor serverAccessor)
        {
            ServerAccessor = serverAccessor;
        }

        [CommandParameter(0, Name = "Name", Description = "Name of the plotter to create")]
        public string Name { get; set; }

        [CommandParameter(1, Name = "Username", Description = "The name of the user used for authentication")]
        public string Username { get; set; }

        [CommandParameter(2, Name = "Password", Description = "The password of the user used for authentication")]
        public string Password { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var creationTask = ServerAccessor.CreatePlotterAsync(Name, Username, Password);
            var plotterResult = await creationTask.Try();

            if (plotterResult == null)
            {
                var response = (creationTask.Exception.InnerException as HttpRequestException)?.StatusCode;
                if (response == null || !response.HasValue)
                {
                    throw creationTask.Exception.InnerException;
                }
                var responseCode = response.Value;

                if (responseCode == HttpStatusCode.NotFound)
                {
                    await ErrorLineAsync("This pool does not support plotter creation!");
                }
                else
                {
                    throw creationTask.Exception.InnerException;
                }

                return;
            }

            await SuccessLineAsync("Successfully created plotter!");
            await TableAsync(new Dictionary<string, Func<CreatePlotterResult, object>>()
            {
                ["Id"] = x => x.Info.Id,
                ["Name"] = x => x.Info.Name,
                ["Token"] = x => x.Token,
            }, new[] { plotterResult });
        }
    }
}
