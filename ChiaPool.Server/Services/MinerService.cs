using ChiaPool.Hubs;
using ChiaPool.Models;
using Common.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class MinerService : Service
    {
        public const int PlotMinutesPerInterval = 3;
        public const int PlotMinuteClaimInterval = PlotMinutesPerInterval * 60 * 1000;

        [Inject]
        private readonly PlotService PlotService;
        [Inject]
        private readonly IHubContext<MinerHub> HubContext;
        [Inject]
        private readonly FirewallService FirewallService;

        private readonly Dictionary<long, (MinerStatus Status, IPAddress Address)> ActiveMiners;
        private readonly SemaphoreSlim ActiveMinerLock;

        public MinerService()
        {
            ActiveMiners = new Dictionary<long, (MinerStatus Status, IPAddress Address)>();
            ActiveMinerLock = new SemaphoreSlim(1, 1);
        }

        protected override async ValueTask RunAsync()
        {
            var claimDelay = Task.Delay(PlotMinuteClaimInterval);
            var activeMinersAtStart = ActiveMiners.ToDictionary(x => x.Key, x => x.Value); //This does not change when the dictionary changes!

            while (true)
            {
                await claimDelay;
                claimDelay = Task.Delay(PlotMinuteClaimInterval);
                await RewardMiners(activeMinersAtStart);
                activeMinersAtStart = ActiveMiners.ToDictionary(x => x.Key, x => x.Value);
            }
        }

        private async Task RewardMiners(IDictionary<long, (MinerStatus Status, IPAddress Address)> activeMiners)
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();
            var minerIds = activeMiners.Keys.ToArray();

            var miners = await dbContext.Miners
                .Include(x => x.Owner)
                .Where(x => minerIds.Contains(x.Id))
                .ToListAsync();

            foreach (var miner in miners)
            {
                var status = activeMiners[miner.Id];
                int pmReward = status.Status.PlotCount * PlotMinutesPerInterval;

                miner.Earnings += pmReward;
                miner.Owner.PlotMinutes += pmReward;

                PlotService.IncrementTotalPlotMinutes(pmReward);
            }

            await dbContext.SaveChangesConcurrentAsync();
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

        public MinerInfo GetMinerInfo(Miner miner) 
            => ActiveMiners.TryGetValue(miner.Id, out var minerStatus)
                ? new MinerInfo(miner.Id, true, minerStatus.Status.PlotCount, miner.Name, miner.Earnings, miner.OwnerId)
                : new MinerInfo(miner.Id, false, -1, miner.Name, miner.Earnings, miner.OwnerId);

        public async Task ActivateMinerAsync(long minerId, MinerStatus status, IPAddress address)
        {
            await ActiveMinerLock.WaitAsync();
            try
            {
                if (!ActiveMiners.TryAdd(minerId, (status, address)))
                {
                    throw new InvalidOperationException("Cannot activate active miner");
                }

                await FirewallService.AcceptIPAsync(address);
                Logger.LogInformation($"Activated miner [{minerId}]");
            }
            finally
            {
                ActiveMinerLock.Release();
            }
        }
        public async Task UpdateMinerAsync(long minerId, MinerStatus status, IPAddress address)
        {
            await ActiveMinerLock.WaitAsync();

            try
            {
                if (!ActiveMiners.TryGetValue(minerId, out var oldValue))
                {
                    throw new InvalidOperationException("Cannot update inactive miner");
                }
                ActiveMiners[minerId] = (status, address);

                await FirewallService.SwapMinerIP(oldValue.Address, address);
                Logger.LogInformation($"Updated miner [{minerId}]");
            }
            finally
            {
                ActiveMinerLock.Release();
            }
        }
        public async Task DeactivateMinerAsync(long minerId)
        {
            await ActiveMinerLock.WaitAsync();

            try
            {
                if (!ActiveMiners.Remove(minerId, out var oldValue))
                {
                    throw new InvalidOperationException("Cannot deactivate inactive miner");
                }

                await FirewallService.DropIPAsync(oldValue.Address);
                Logger.LogInformation($"Deactivated miner [{minerId}]");
            }
            finally
            {
                ActiveMinerLock.Release();
            }
        }
    }
}
