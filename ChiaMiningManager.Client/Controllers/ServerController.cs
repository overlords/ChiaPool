using ChiaMiningManager.Configuration;
using ChiaMiningManager.Configuration.Options;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChiaMiningManager.Controllers
{
    [Route("Server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly HttpClient Client;
        private readonly ServerOptions ServerOptions;
        private readonly AuthOption AuthOptions;

        public ServerController(HttpClient client, ServerOptions serverOptions, AuthOption authOptions)
        {
            Client = client;
            ServerOptions = serverOptions;
            AuthOptions = authOptions;
        }

        [HttpGet("Info/Wallet")]
        public async Task<IActionResult> GetWalletAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{ServerOptions.PoolHost}:{ServerOptions.ManagerPort}/Info/Status");
            request.Headers.Authorization = new AuthenticationHeaderValue(AuthOptions.Token);
            var response = await Client.SendAsync(request);
            object result = null;

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return StatusCode((int)response.StatusCode, result);
        }


    }
}
