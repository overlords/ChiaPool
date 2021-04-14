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
    public class MinerHub : Hub
    {
        private readonly MinerService MinerService;

        public MinerHub(MinerService minerService)
        {
            MinerService = minerService;
        }

        [HubMethodName(MinerHubMethods.Activate)]
        public Task<ActivationResult> ActivateAsync(MinerStatus status)
        {
            long minerId = long.Parse(Context.UserIdentifier);
            var address = GetRequestIP();
            return MinerService.ActivateMinerAsync(Context.ConnectionId, minerId, status, address);
        }

        [HubMethodName(MinerHubMethods.Update)]
        public async Task UpdateMinerAsync(MinerStatus status)
        {
            long minerId = long.Parse(Context.UserIdentifier);
            await MinerService.UpdateMinerAsync(Context.ConnectionId, minerId, status);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            long minerId = long.Parse(Context.UserIdentifier);
            await MinerService.DeactivateMinerAsync(Context.ConnectionId, minerId);
        }

        private IPAddress GetRequestIP()
        => !Context.GetHttpContext().Request.Headers.TryGetValue("HTTP_X-FORWARDED-FOR", out var value)
            ? null
            : IPAddress.Parse(value);
    }
}
