namespace ChiaPool.Models
{
    public class PlotterInfo : INodeInfo
    {
        public long Id { get; init; }
        public bool Online { get; init; }
        public string Name { get; init; }

        public long LastCapacity { get; init; }
        public long PlotMinutes { get; init; }
        public long OwnerId { get; init; }

        public PlotterInfo()
        {
        }
        public PlotterInfo(long id, bool online, string name, long lastCapacity, long plotMinutes, long ownerId)
        {
            Id = id;
            Online = online;
            Name = name;
            LastCapacity = lastCapacity;
            PlotMinutes = plotMinutes;
            OwnerId = ownerId;
        }
    }
}
