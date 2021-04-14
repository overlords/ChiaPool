namespace ChiaPool.Models
{
    public sealed class PlotterActivation
    {
        public string ConnectionId { get; set; }
        public PlotterStatus Status { get; set; }

        public PlotterActivation(string connectionId, PlotterStatus status)
        {
            ConnectionId = connectionId;
            Status = status;
        }
    }
}
