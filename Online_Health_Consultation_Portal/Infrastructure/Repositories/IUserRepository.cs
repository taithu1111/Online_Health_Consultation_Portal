using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserWithProfileAsync(int userId);
        Task UpdateUserProfileAsync(User user);
    }
}