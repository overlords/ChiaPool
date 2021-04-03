using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaPool.Commands.Logs
{
    [Command("Log Generate", Description = "Retrieves the log of the current / last plot generation")]
    public class LogGenerationCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;

        protected override Task ExecuteAsync(IConsole console)
        {
            throw new NotImplementedException();
        }
    }
}
