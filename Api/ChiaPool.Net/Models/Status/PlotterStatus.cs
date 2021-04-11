namespace ChiaPool.Models
{
    public class PlotterStatus : IStatus
    {
        public int Capacity { get; init; }
        public int PlotsAvailable { get; init; }

        public PlotterStatus()
        {
        }
    }
}
