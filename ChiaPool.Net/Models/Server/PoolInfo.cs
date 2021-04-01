namespace ChiaPool.Models.Server
{
    public sealed class PoolInfo
    {
        public string Name { get; init; }

        public int TotalMinerCount { get; init; }
        public int ActiveMinerCount { get; init; }

        public int TotalPlotCount { get; init; }
        public int ActivePlotCount { get; init; }

        public PoolInfo()
        {
        }
    }
}
