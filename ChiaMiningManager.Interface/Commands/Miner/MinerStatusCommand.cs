using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands
{
    [Command("Miner Status")]
    public class MinerStatusCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {


            return ValueTask.CompletedTask;
        }
    }
}
