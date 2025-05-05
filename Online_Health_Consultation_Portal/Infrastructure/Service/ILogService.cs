namespace Online_Health_Consultation_Portal.Infrastructure.Service
{
    public interface ILogService
    {
        Task LogInformationAsync(string message, string userId, string action, string entity, int entityId);
        Task LogWarningAsync(string message, string userId, string action, string entity, int entityId);
        Task LogErrorAsync(string message, string userId, string action, string entity, int entityId);
    }
}
