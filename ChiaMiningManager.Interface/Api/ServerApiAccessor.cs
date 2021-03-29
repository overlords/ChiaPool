using System.Net.Http;

namespace ChiaMiningManager.Api
{
    public class ServerApiAccessor
    {
        private readonly HttpClient Client;

        public ServerApiAccessor(HttpClient client)
        {
            Client = client;
        }
    }
}
