using ChiaPool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChiaPool.Hubs
{
    [Authorize(AuthenticationSchemes = "Miner")]
    public class MinerHub : Hub<IMinerClient>
    {
    }
}
