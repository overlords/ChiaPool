using ChiaMiningManager.Configuration;
using Common.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChiaMiningManager.Services
{
    public class MiningNotifier : Service
    {
        [Inject]
        private readonly AuthOption AuthOptions;
        [Inject]
        private readonly HttpClient Client;

        private const int TimeBetweenClaims = 60 * 1000;

        protected override async ValueTask RunAsync()
        {
            while (true)
            {
                await Task.Delay(TimeBetweenClaims);

                try
                {
                    await SendClaimRequestAsync();
                }
                catch (HttpRequestException)
                {
                    Logger.LogError("Could not claim mined time: Could not reach server, are you connected to the internet?");
                    throw;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occured while claiming mined time");
                }
            }
        }

        private async Task SendClaimRequestAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "pool.playwo.de/miner/claim");
            request.Headers.Authorization = new AuthenticationHeaderValue(AuthOptions.Token);

            var response = await Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    Logger.LogCritical("Could not claim mined time: Invalid token!");
                    break;
                case HttpStatusCode.Conflict:
                    Logger.LogError("Could not claim mined time: Not enough time has passed since last claim. Are you using the same token on multiple devices?");
                    break;
                default:
                    Logger.LogError($"Could not claim mined time: Server responded with {response.StatusCode}. Try updating your image");
                    break;
            }
        }

    }
}
