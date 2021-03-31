using Chia.NET.Models;
using ChiaMiningManager.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChiaMiningManager.Api
{
    public class ClientApiAccessor : ApiAccessor
    {
        private const string ApiUrl = "http://localhost:8888/";

        public ClientApiAccessor(HttpClient client)
            : base(client)
        {
        }

        public Task<ClientStatus> GetStatusAsync()
            => Client.GetFromJsonAsync<ClientStatus>(ClientRoutes.Status(ApiUrl));

        public Task<PlotInfo[]> GetPlotsAsync()
            => Client.GetFromJsonAsync<PlotInfo[]>(ClientRoutes.ListPlots(ApiUrl));

        public Task<bool> DeletePlotByPublicKeyAsync(string publicKey)
            => PostAsync<bool>(ClientRoutes.DeletePlotById(ApiUrl), new Dictionary<string, string>()
            {
                ["publicKey"] = publicKey.ToString(),
            });

        public Task<bool> DeletePlotByFileNameAsync(string fileName)
            => PostAsync<bool>(ClientRoutes.DeletePlotById(ApiUrl), new Dictionary<string, string>()
            {
                ["fileName"] = fileName,
            });

        public Task<Wallet> GetWalletBalanceAsync()
            => Client.GetFromJsonAsync<Wallet>(ClientRoutes.ServerWalletInfo(ApiUrl));

    }
}
