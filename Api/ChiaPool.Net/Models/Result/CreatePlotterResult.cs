namespace ChiaPool.Models
{
    public sealed class CreatePlotterResult
    {

        public string Token { get; init; }
        public PlotterInfo Info { get; init; }

        public CreatePlotterResult(string token, PlotterInfo info)
        {
            Token = token;
            Info = info;
        }
    }
}
