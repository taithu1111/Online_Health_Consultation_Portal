namespace Online_Health_Consultation_Portal.Infrastructure.Services
{
    public interface IUserManagementService
    {
        // Task SoftDeleteUserAsync(int userId);
        Task PermanentlyDeleteUserAsync(int userId);
    }
}