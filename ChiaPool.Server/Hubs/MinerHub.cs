using ChiaPool.Clients;
using ChiaPool.Models;
using ChiaPool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ChiaPool.Hubs
{
    [Authorize(AuthenticationSchemes = "Miner")]
    public class MinerHub : Hub
    {
        private readonly MinerService MinerService;
        private readonly UserService UserService;

        public MinerHub(MinerService minerService, UserService userService)
        {
            MinerService = minerService;
            UserService = userService;
        }

        [HubMethodName(MinerHubMethods.Activate)]
        public async Task<long> ActivateMinerAsync(MinerStatus status)
        {
            long minerId = long.Parse(Context.UserIdentifier);
            var address = GetRequestIP();
            await MinerService.ActivateMinerAsync(minerId, status, address);
            return await UserService.GetOwnerIdFromMinerId(minerId);
        }

        [HubMethodName(MinerHubMethods.Update)]
        public async Task UpdateMinerAsync(MinerStatus status)
        {
            long minerId = long.Parse(Context.UserIdentifier);
            var address = GetRequestIP();
            await MinerService.UpdateMinerAsync(minerId, status, address);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            long minerId = long.Parse(Context.UserIdentifier);
            await MinerService.DeactivateMinerAsync(minerId);
        }

        private IPAddress GetRequestIP()
        => !Context.GetHttpContext().Request.Headers.TryGetValue("HTTP_X-FORWARDED-FOR", out var value)
            ? null
            : IPAddress.Parse(value);
    }
}
