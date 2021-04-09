namespace ChiaPool.Models
{
    public class MinerInfo : INodeInfo
    {
        public long Id { get; init; }
        public bool Online { get; init; }
        public string Name { get; init; }

        public short LastPlotCount { get; set; }
        public long PlotMinutes { get; set; }
        public long OwnerId { get; init; }

        public MinerInfo()
        {
        }
        public MinerInfo(long id, bool online, string name, short lastPlotCount, long plotMinutes, long ownerId)
        {
            Id = id;
            Online = online;
            Name = name;
            LastPlotCount = lastPlotCount;
            PlotMinutes = plotMinutes;
            OwnerId = ownerId;
        }
    }
}
