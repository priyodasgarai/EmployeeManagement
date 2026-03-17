using Employee.Interfaces;

namespace Employee.Service
{
    public class SerilogLoggerAdapter<T>(ILogger<T> _logger) : IAppLogger<T>
    {
        public void LogError(Exception ex, string message) => _logger.LogError(ex, message);
        public void LogInformation(string message) => _logger.LogInformation(message);
        public void LogWaning(string message) => _logger.LogInformation(message);

    }
}
