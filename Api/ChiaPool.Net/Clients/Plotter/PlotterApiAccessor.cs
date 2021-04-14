using System.Net.Http;

namespace ChiaPool.Api
{
    public sealed class PlotterApiAccessor : ApiAccessor
    {
        public PlotterApiAccessor(HttpClient client)
            : base(client)
        {
        }
    }
}
