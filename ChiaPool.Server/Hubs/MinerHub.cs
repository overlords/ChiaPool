using ChiaPool.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChiaPool.Hubs
{
    public class MinerHub : Hub<IMinerClient>
    {
    }
}
