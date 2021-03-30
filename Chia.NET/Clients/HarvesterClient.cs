using System.Net.Http;

namespace Chia.NET.Clients
{
    public sealed class HarvesterClient
    {
        private readonly HttpClient Client;

        public HarvesterClient(HttpClient client)
        {
            Client = client;
        }


    }
}
