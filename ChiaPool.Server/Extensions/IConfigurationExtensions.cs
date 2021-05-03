using Microsoft.Extensions.Configuration;

namespace ChiaPool.Extensions
{
    public static partial class Extensions
    {
        public static bool IsSwaggerEnabled(this IConfiguration configuration) 
            => configuration.GetValue<bool>("EnableSwagger");
    }
}
