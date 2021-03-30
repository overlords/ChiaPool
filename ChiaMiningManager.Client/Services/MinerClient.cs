using ChiaMiningManager.Configuration;
using Common.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChiaMiningManager.Services
{
    public class MinerClient : Service
    {
        [Inject]
        private readonly HttpClient Client;

        [Inject]
        private readonly AuthOption AuthOptions;

        public async Task<bool> SendStartRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pool.playwo.de/miner/start");
            request.Headers.Authorization = new AuthenticationHeaderValue(AuthOptions.Token);

            try
            {
                var response = await Client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Logger.LogInformation("Successfully started mining session");
                    return true;
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
                        Logger.LogError($"Could not start mining session: The server responded with {response.StatusCode}!");
                        break;
                }
            }
            catch (HttpRequestException)
            {
                Logger.LogError($"Could not start mining session: There was a connection error, are you connected to the internet?");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Could not start mining session: An error occured!");
            }

            return false;
        }

        public async Task<bool> RefreshCAKeysAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://pool.playwo.de/cert/ca");
            request.Headers.Authorization = new AuthenticationHeaderValue(AuthOptions.Token);

            try
            {
                var response = await Client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();
                    RefreshCAKeysFromStream(data);
                    Logger.LogInformation("Successfully updated certificates");
                    return true;
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        Logger.LogCritical("Could not start update keys: Your miner token is invalid!");
                        break;
                    default:
                        Logger.LogError($"Could not start update keys: The server responded with {response.StatusCode}");
                        break;
                }
            }
            catch (HttpRequestException)
            {
                Logger.LogError($"Could not start update keys: There was a connection error, are you connected to the internet?");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Could not start update keys: An error occured!");
            }


            return false;
        }

        private void RefreshCAKeysFromStream(Stream data)
        {
            using var archive = new ZipArchive(data);

            foreach (var entry in archive.Entries)
            {
                string path = $"/root/chia-blockchain/ca/{entry.FullName}";
                entry.ExtractToFile(path, true);
            }
        }
    }
}
