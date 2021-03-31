using ChiaMiningManager.Configuration.Options;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChiaMiningManager.Controllers
{
    [Route("Server")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly HttpClient Client;
        private readonly ServerOptions ServerOptions;

        public ServerController(HttpClient client, ServerOptions serverOptions)
        {
            Client = client;
            ServerOptions = serverOptions;
        }

        [HttpGet("Info/Status")]
        public async Task<IActionResult> GetServerStatusAsync()
        {
            var response = await Client.GetAsync($"{ServerOptions.PoolHost}:{ServerOptions.ManagerPort}/Info/Status");
            object result = null;

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return StatusCode((int)response.StatusCode, result);
        }


    }
}
