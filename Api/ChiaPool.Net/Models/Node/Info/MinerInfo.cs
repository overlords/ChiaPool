namespace ChiaPool.Models
{
    public class MinerInfo : INodeInfo
    {
        public long Id { get; init; }
        public bool Online { get; init; }
        public int PlotCount { get; init; }
        public string Name { get; init; }

        public long Earnings { get; set; }
        public long OwnerId { get; init; }

        public MinerInfo()
        {
        }
        public MinerInfo(long id, bool online, int plotCount, string name, long totalEarnings, long ownerId)
        {
            Id = id;
            Online = online;
            Name = name;
            PlotCount = plotCount;
            Earnings = totalEarnings;
            OwnerId = ownerId;
        }
    }
}
