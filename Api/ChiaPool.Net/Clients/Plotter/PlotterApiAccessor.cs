using System.Net.Http;

namespace ChiaPool.Api
{
    public sealed class PlotterApiAccessor : ApiAccessor
    {
        private string ApiUrl;

        public PlotterApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public void SetApiUrl(string apiUrl)
        {
            ApiUrl = apiUrl;
        }


    }
}
