using Chia.NET.Models;
using System.Threading.Tasks;

namespace Chia.NET.Clients
{
    public sealed class FullNodeClient : ChiaApiClient
    {
        private const string ApiUrl = "https://localhost:8555/";

        public FullNodeClient() 
            : base("full_node", ApiUrl)
        {
        }

        public async Task<BlockchainState> GetBlockchainStateAsync()
        {
            var result = await PostAsync<GetBlockchainStateResult>(FullNodeRoutes.GetBlockchainState(ApiUrl));
            return result.BlockchainState;
        }
    }
}
