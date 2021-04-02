using ChiaPool.Models;

namespace ChiaPool.Extensions
{
    public static class MinerExtensions
    {
        public static Miner WithoutSecrets(this Miner miner)
        {
            miner.LastAddress = default;
            miner.Token = default;
            return miner;
        }
    }
}
