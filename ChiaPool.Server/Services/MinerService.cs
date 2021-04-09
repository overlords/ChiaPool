using ChiaPool.Hubs;
using ChiaPool.Models;
using Common.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class MinerService : Service
    {
        public const int PlotMinutesPerInterval = 5;
        public const int PlotMinuteClaimInterval = PlotMinutesPerInterval * 60 * 1000;

        [Inject]
        private readonly IHubContext<MinerHub> HubContext;
        [Inject]
        private readonly FirewallService FirewallService;

        private readonly ConcurrentDictionary<long, (MinerStatus Status, IPAddress Address)> ActiveMiners;


        protected override async ValueTask RunAsync()
        {
            var claimDelay = Task.Delay(PlotMinuteClaimInterval);
            var activeMinersAtStart = ActiveMiners; //This does not change when the dictionary changes!
            while (true)
            {
                await claimDelay;
                claimDelay = Task.Delay(PlotMinuteClaimInterval);
                await RewardMiners(activeMinersAtStart);
                activeMinersAtStart = ActiveMiners;
            }
        }

        private async Task RewardMiners(IDictionary<long, (MinerStatus Status, IPAddress Address)> activeMiners)
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();
            var minerIds = activeMiners.Keys.ToArray();

            var miners = await dbContext.Miners
                .Where(x => minerIds.Contains(x.Id))
                .ToListAsync();

            foreach (var miner in miners)
            {
                var status = activeMiners[miner.Id];
                miner.PlotMinutes += status.Status.PlotCount;
            }
            await dbContext.SaveChangesAsync();
        }

        public Task<int> GetTotalMinerCount()
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();
            return dbContext.Miners.CountAsync();
        }
        public int GetActiveMinerCount()
            => ActiveMiners.Count;

        public int GetActivePlotCount()
            => ActiveMiners.Sum(x => x.Value.Status.PlotCount);

        public bool IsMinerActive(long minerId)
            => ActiveMiners.TryGetValue(minerId, out _);

        public MinerInfo GetMinerInfo(Miner miner)
            => new MinerInfo(miner.Id, IsMinerActive(miner.Id), miner.Name, miner.LastPlotCount, miner.PlotMinutes, miner.OwnerId);

        public async Task ActivateMinerAsync(long minerId, MinerStatus status, IPAddress address)
        {
            if (!ActiveMiners.TryAdd(minerId, (status, address)))
            {
                throw new InvalidOperationException("Cannot activate active miner");
            }

            await FirewallService.AcceptIPAsync(address);
        }
        public async Task UpdateMinerAsync(long minerId, MinerStatus status, IPAddress address)
        {
            if(!ActiveMiners.TryGetValue(minerId, out var oldValue))
            {
                throw new InvalidOperationException("Cannot update inactive plotter");
            }
            if(!ActiveMiners.TryUpdate(minerId, (status, address), (status, address)))
            {
                throw new InvalidOperationException("Cannot update inactive plotter");
            }

            await FirewallService.SwapMinerIP(oldValue.Address, address);
        }

        public async Task DeactivateMinerAsync(long minerId)
        {
            if (!ActiveMiners.TryRemove(minerId, out var value))
            {
                throw new InvalidOperationException("Cannot deactivate inactive plotter");
            }

            await FirewallService.DropIPAsync(value.Address);
        }
    }
}
