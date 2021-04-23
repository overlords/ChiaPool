using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class AuthOption : Option
    {
        public string Token { get; init; } = Environment.GetEnvironmentVariable("token");

        public string FarmerKeys { get; init; } = Environment.GetEnvironmentVariable("farmer_keys");

        protected override async ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (!Guid.TryParse(Token, out _))
            {
                return ValidationResult.Failed("Miner token has invalid format!");
            }
            if (FarmerKeys.Split(' ').Length != 24)
            {
                return ValidationResult.Failed($"Invalid farmer keys! Excpected 24 words, got {FarmerKeys}");
            }

            return await base.ValidateAsync(provider);
        }
    }
}
