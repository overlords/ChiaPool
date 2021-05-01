using ChiaPool.Models;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly HashSet<PlotInfo> PlotInfos;
        private readonly SemaphoreSlim MinerLock;

        public MinerService()
        {
            ActiveMiners = new Dictionary<long, MinerActivation>();
            PlotInfos = new HashSet<PlotInfo>();
            MinerLock = new SemaphoreSlim(1, 1);
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

        public async Task<int> GetActivePlotCountAsync()
        {
            await MinerLock.WaitAsync();
            try
            {
                return ActiveMiners.Sum(x => x.Value.Status.PlotCount);
            }
            finally
            {
                MinerLock.Release();
            }
        }
        public async Task<MinerInfo> GetMinerInfoAsync(Miner miner)
        {
            await MinerLock.WaitAsync();
            try
            {
                return ActiveMiners.TryGetValue(miner.Id, out var minerStatus)
                   ? new MinerInfo(miner.Id, true, minerStatus.Status.PlotCount, miner.Name, miner.Earnings, miner.OwnerId)
                   : new MinerInfo(miner.Id, false, -1, miner.Name, miner.Earnings, miner.OwnerId);
            }
            finally
            {
                MinerLock.Release();
            }
        }

        public async Task<MinerActivationResult> ActivateMinerAsync(string connectionId, long minerId, MinerStatus status, List<PlotInfo> plotInfos)
        {
            await MinerLock.WaitAsync();

            try
            {
                if (plotInfos.Count != status.PlotCount)
                {
                    Logger.LogWarning($"Miner [{minerId}] tried to activate with unmatching status and plotInfos plot count");
                    return MinerActivationResult.FromStatusPlotCountUnmatch();
                }
                if (plotInfos.Count != plotInfos.Distinct().Count())
                {
                    Logger.LogWarning($"Miner [{minerId}] tried to activate with duplicate plot public keys");
                    return MinerActivationResult.FromDuplicates();
                }
                if (ActiveMiners.ContainsKey(minerId))
                {
                    return MinerActivationResult.FromAlreadyActive();
                }

                var conflicts = AddPlotInfosAndFilterConflicts(plotInfos);

                if (conflicts.Any())
                {
                    Logger.LogWarning($"Miner [{minerId}] tried to activate with {conflicts.Count} conflicing plots!");
                    status = new MinerStatus(status.PlotCount - conflicts.Count);
                }

                var activation = new MinerActivation(connectionId, status, plotInfos);
                ActiveMiners.Add(minerId, activation);

                long userId = await UserService.GetOwnerIdFromMinerId(minerId);

                Logger.LogInformation($"Activated miner [{minerId}]");

                return conflicts.Any()
                    ? MinerActivationResult.FromConflicingPlots(userId, conflicts.ToArray())
                    : MinerActivationResult.FromSuccess(userId);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "There was an exception while activating a miner!");
                return MinerActivationResult.FromError();
            }
            finally
            {
                MinerLock.Release();
            }
        }
        public async Task<MinerUpdateResult> UpdateMinerAsync(string connectionId, long minerId, MinerStatus status, List<PlotInfo> plotInfos)
        {
            await MinerLock.WaitAsync();

            try
            {
                if (!ActiveMiners.TryGetValue(minerId, out var oldValue))
                {
                    throw new InvalidOperationException("Cannot update inactive miner");
                }
                if (oldValue.ConnectionId != connectionId)
                {
                    Logger.LogWarning($"Miner [{minerId}] tried to update state from a different connection");
                    return MinerUpdateResult.FromInvalidConnection();
                }

                RemovePlotInfos(oldValue.PlotInfos);
                var conflicts = AddPlotInfosAndFilterConflicts(plotInfos);

                if (conflicts.Any())
                {
                    Logger.LogWarning($"Miner [{minerId}] tried to update with {conflicts.Count} conflicing plots!");
                    status = new MinerStatus(status.PlotCount - conflicts.Count);
                }

                oldValue.Update(status, plotInfos);
                Logger.LogInformation($"Updated miner [{minerId}]");

                return conflicts.Any()
                    ? MinerUpdateResult.FromConflicingPlots(conflicts.ToArray())
                    : MinerUpdateResult.FromSuccess();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "There was an excpetion while updating a miner!");
                return MinerUpdateResult.FromError();
            }
            finally
            {
                MinerLock.Release();
            }
        }
        public async Task DeactivateMinerAsync(string connectionId, long minerId)
        {
            await MinerLock.WaitAsync();

            try
            {
                if (!ActiveMiners.TryGetValue(minerId, out var oldValue))
                {
                    return;
                }
                if (oldValue.ConnectionId != connectionId)
                {
                    return;
                }

                RemovePlotInfos(oldValue.PlotInfos);

                ActiveMiners.Remove(minerId);
                Logger.LogInformation($"Deactivated miner [{minerId}]");
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "There was an excpetion while deactivating a miner!");
            }
            finally
            {
                MinerLock.Release();
            }
        }

        private void RemovePlotInfos(List<PlotInfo> plotInfos) //May only be called inside of MinerLock
        {
            foreach(var plotInfo in plotInfos)
            {
                PlotInfos.Remove(plotInfo);
            }
        }
        private List<PlotInfo> AddPlotInfosAndFilterConflicts(List<PlotInfo> plotInfos) //May only be called inside of MinerLock
        {
            List<PlotInfo> conflicingPlots = new List<PlotInfo>();
            foreach (var plotInfo in plotInfos.ToArray())
            {
                if (PlotInfos.Add(plotInfo))
                {
                    continue;
                }

                conflicingPlots.Add(plotInfo);
                plotInfos.Remove(plotInfo);
            }
            return conflicingPlots;
        }
    }
}
