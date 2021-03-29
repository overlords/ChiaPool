using ChiaMiningManager.Configuration;
using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChiaMiningManager.Services
{
    public class MiningNotifier : Service
    {
        private const int TimeBetweenClaimRequests = 60 * 1000; //1 Minute

        [Inject]
        private readonly HttpClient Client;

        [Inject]
        private readonly AuthOption AuthOptions;

        protected override async ValueTask RunAsync()
        {
            await SendStartRequest();

            var delayTask = Task.Delay(TimeBetweenClaimRequests);
            var scope = Provider.CreateScope();
            while (true)
            {
                await delayTask;
                delayTask = Task.Delay(TimeBetweenClaimRequests);

                var plotManager = scope.ServiceProvider.GetRequiredService<PlotManager>();
                await SendClaimRequest(plotManager);
                await plotManager.IncrementPlots();
            }
        }

        private async Task SendStartRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://pool.playwo.de:8666/miner/start");
            request.Headers.Authorization = new AuthenticationHeaderValue(AuthOptions.Token);

            try
            {
                var response = await Client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Logger.LogInformation("Successfully started mining session!");
                    return;
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        Logger.LogCritical("Could not start mining session: The server could not identify your ip!");
                        break;
                    case HttpStatusCode.Unauthorized:
                        Logger.LogCritical("Could not start mining session: Your miner token is invalid!");
                        break;
                    default:
                        Logger.LogError($"Could not start mining session: The server responded with {response.StatusCode}");
                        break;
                }
            }
            catch (HttpRequestException)
            {
                Logger.LogError($"Could not start mining session: There was a connection error, are you connected to the internet?");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Could not start mining session: An error occured");
            }
        }

        private async Task SendClaimRequest(PlotManager plotManager)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "pool.playwo.de/miner/claim");
            request.Headers.Authorization = new AuthenticationHeaderValue(AuthOptions.Token);

            int plotCount = await plotManager.GetPlotsCountAsync();

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["activePlots"] = $"{plotCount}",
            });

            try
            {
                var response = await Client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        Logger.LogCritical("Could not claim PM: The server could not identify your ip!");
                        break;
                    case HttpStatusCode.Unauthorized:
                        Logger.LogCritical("Could not claim PM: Your miner token is invalid!");
                        break;
                    case HttpStatusCode.Conflict:
                        Logger.LogError("Could not claim PM: Are you using the same token for multiple miners?");
                        break;
                    default:
                        Logger.LogError($"Could not claim PM: The server responded with {response.StatusCode}");
                        break;
                }
            }
            catch (HttpRequestException)
            {
                Logger.LogError($"Could not claim PM: There was a connection error, are you connected to the internet?");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Could not claim PM: An error occured");
            }
        }
    }
}
