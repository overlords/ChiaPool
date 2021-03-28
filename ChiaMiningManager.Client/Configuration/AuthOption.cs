using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaMiningManager.Configuration
{
    [SectionName("Auth")]
    public sealed class AuthOption : Option
    {
        public string Token { get; set; }

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            return !Guid.TryParse(Token, out _)
                ? ValueTask.FromResult(ValidationResult.Failed("Invalid Token Format"))
                : ValueTask.FromResult(ValidationResult.Success);
        }
    }
}
