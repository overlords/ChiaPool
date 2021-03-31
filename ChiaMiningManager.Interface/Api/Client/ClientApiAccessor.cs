using Chia.NET.Models;
using System;
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

        public Task<Plot[]> GetPlotsAsync()
            => Client.GetFromJsonAsync<Plot[]>(ClientRoutes.ListPlots(ApiUrl));

        //public Task<bool> DeletePlotByIdAsync(Guid id)
        //    => PostAsync<bool>(ClientRoutes.DeletePlotById(ApiUrl), new Dictionary<string, string>()
        //    {
        //        ["id"] = id.ToString(),
        //    });

        //public Task<bool> DeletePlotByNameAsync(string name)
        //    => PostAsync<bool>(ClientRoutes.DeletePlotById(ApiUrl), new Dictionary<string, string>()
        //    {
        //        ["name"] = name,
        //    });

    }
}
