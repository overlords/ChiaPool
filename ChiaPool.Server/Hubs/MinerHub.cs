﻿using ChiaPool.Clients;
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
    public class MinerHub : Hub<IMinerClient>
    {
        private readonly MinerService MinerService;

        public MinerHub(MinerService minerService)
        {
            MinerService = minerService;
        }

        [HubMethodName(MinerHubMethods.Activate)]
        public async Task ActivateMinerAsync(MinerStatus status)
        {
            long minerId = long.Parse(Context.UserIdentifier);
            var address = GetRequestIP();
            await MinerService.ActivateMinerAsync(minerId, status, address);
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
