using Chia.NET.Clients;
using Common.Services;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public sealed class BlockValidationService : Service
    {
        [Inject] private readonly FullNodeClient FullNodeClient;

        protected override ValueTask InitializeAsync()
        {
            //ToDo: Handle miners winning blocks while the pool if offline
            // => Paying back the farmer rewards 

            return base.InitializeAsync();
        }

        protected override ValueTask RunAsync()
        {

            return ValueTask.CompletedTask;
        }


    }
}
