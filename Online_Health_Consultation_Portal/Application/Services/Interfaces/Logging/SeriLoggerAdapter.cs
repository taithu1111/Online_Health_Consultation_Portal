namespace Online_Health_Consultation_Portal.Application.Services.Interfaces.Logging
{
    public class SeriLoggerAdapter<T>(ILogger<T> logger) : IApplogger<T>
    {
        public void LogError(Exception ex , string message) => logger.LogError(ex, message);

        public void LogInformation(string message) => logger.LogInformation(message);

        public void LogWảning(string message) => logger.LogWarning(message);
    }
}
