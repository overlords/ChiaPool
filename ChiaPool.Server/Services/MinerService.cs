using ChiaPool.Models;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly UserService UserService;

        private readonly Dictionary<long, MinerActivation> ActiveMiners;
        private readonly SemaphoreSlim ActiveMinersLock;

        public MinerService()
        {
            ActiveMiners = new Dictionary<long, MinerActivation>();
            ActiveMinersLock = new SemaphoreSlim(1, 1);
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

        private async Task RewardMiners(Dictionary<long, MinerActivation> activeMiners)
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

        public Task<int> GetTotalMinerCountAsync()
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

        public async Task<MinerActivationResult> ActivateMinerAsync(string connectionId, long minerId, MinerStatus status, PlotInfo[] plotInfos)
        {
            await ActiveMinersLock.WaitAsync();
            try
            {
                var activation = new MinerActivation(connectionId, status, plotInfos);
                if (!ActiveMiners.TryAdd(minerId, activation))
                {
                    return MinerActivationResult.FromFailed("There already is a active connection from this miner!");
                }

                Logger.LogInformation($"Activated miner [{minerId}]");
                long userId = await UserService.GetOwnerIdFromMinerId(minerId);
                return MinerActivationResult.FromSuccess(userId);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "There was an exception while activating a miner!");
                return MinerActivationResult.FromFailed("An unknown error occurred!");
            }
            finally
            {
                ActiveMinersLock.Release();
            }
        }
        public async Task<MinerUpdateResult> UpdateMinerAsync(string connectionId, long minerId, MinerStatus status, PlotInfo[] plotInfos)
        {
            await ActiveMinersLock.WaitAsync();

            try
            {
                if (!ActiveMiners.TryGetValue(minerId, out var oldValue))
                {
                    throw new InvalidOperationException("Cannot update inactive miner");
                }
                if (oldValue.ConnectionId != connectionId)
                {
                    throw new InvalidOperationException("Cannot update active miner from different connection");
                }

                oldValue.Update(status, plotInfos);
                ActiveMiners[minerId] = oldValue;
                Logger.LogInformation($"Updated miner [{minerId}]");
                return MinerUpdateResult.FromSuccess();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "There was an excpetion while updating a miner!");
                return MinerUpdateResult.FromFailed("An unknown error occurred!");
            }
            finally
            {
                ActiveMinersLock.Release();
            }
        }
        public async Task DeactivateMinerAsync(string connectionId, long minerId)
        {
            await ActiveMinersLock.WaitAsync();

            try
            {
                if (!ActiveMiners.TryGetValue(minerId, out var oldValue))
                {
                    throw new InvalidOperationException("Cannot deactivate inactive miner");
                }
                if (oldValue.ConnectionId != connectionId)
                {
                    Logger.LogWarning("Cannot deactivate miner from different connection");
                    return;
                    //throw new InvalidOperationException("Cannot deactivate miner from different connection");
                }

                ActiveMiners.Remove(minerId);
                Logger.LogInformation($"Deactivated miner [{minerId}]");
            }
            finally
            {
                ActiveMinersLock.Release();
            }
        }
    }
}
