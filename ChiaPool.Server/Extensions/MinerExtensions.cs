using ChiaPool.Models;

namespace ChiaPool.Extensions
{
    public static class MinerExtensions
    {
        public static Miner WithoutSecrets(this Miner miner)
            => new Miner()
            {
                Id = miner.Id,
                Name = miner.Name,
                PlotMinutes = miner.PlotMinutes,
            };
    }
}
