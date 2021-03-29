using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaMiningManager.Configuration
{
    public class AuthOption : Option
    {
        public string Token { get; set; } = "";

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            Token = Environment.GetEnvironmentVariable("token");

            return !Guid.TryParse(Token, out _)
                ? ValueTask.FromResult(ValidationResult.Failed("Miner token has invalid format!"))
                : base.ValidateAsync(provider);
        }
    }
}
