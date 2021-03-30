using Chia.NET.Clients;
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
        private readonly HarvesterClient HarvesterClient;

        [Inject]
        private readonly AuthOption AuthOptions;

        protected override async ValueTask RunAsync()
        {
            var delayTask = Task.Delay(TimeBetweenClaimRequests);
            while (true)
            {
                await delayTask;
                delayTask = Task.Delay(TimeBetweenClaimRequests);

                using var scope = Provider.CreateScope();
                var plotManager = scope.ServiceProvider.GetRequiredService<PlotManager>();
                await SendClaimRequest(plotManager);
                await plotManager.IncrementPlots();
            }
        }

        private async Task SendClaimRequest(PlotManager plotManager)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pool.playwo.de/miner/claim");
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

        private async Task<int> GetPlotCount()
        {
            var plots = //await HarvesterClient.GetPlotsAsync();

            return plots.Count;
        }
    }
}
