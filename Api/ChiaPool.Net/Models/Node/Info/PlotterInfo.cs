namespace ChiaPool.Models
{
    public class PlotterInfo : INodeInfo
    {
        public long Id { get; init; }
        public bool Online { get; init; }
        public int Capacity { get; init; }
        public int PlotsAvailable { get; init; }
        public string Name { get; init; }

        public long Earnings { get; init; }
        public long OwnerId { get; init; }

        public PlotterInfo()
        {
        }
        public PlotterInfo(long id, bool online, int capacity, int plotsAvailable, string name, long earnings, long ownerId)
        {
            Id = id;
            Online = online;
            Capacity = capacity;
            PlotsAvailable = plotsAvailable;
            Name = name;
            Earnings = earnings;
            OwnerId = ownerId;
        }
    }
}
