namespace Online_Health_Consultation_Portal.Application.Services.Interfaces.Logging
{
    public interface IApplogger<T>
    {
        void LogInformation(string message);
        void LogWảning(string message);
        void LogError(Exception ex, string message);

    }
}
