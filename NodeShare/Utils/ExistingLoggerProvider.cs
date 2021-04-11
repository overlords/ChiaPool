using Microsoft.Extensions.Logging;

namespace ChiaPool.Utils
{
    public class ExistingLoggerProvider : ILoggerProvider
    {
        public ExistingLoggerProvider(ILogger logger)
        {
            _logger = logger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Dispose()
        {
            return;
        }

        private readonly ILogger _logger;
    }
}
