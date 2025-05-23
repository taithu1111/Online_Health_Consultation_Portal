using Online_Health_Consultation_Portal.Domain.Entities;

namespace Online_Health_Consultation_Portal.Application.Interfaces.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<bool> ValidateRefreshTokenAsync(User user, string refreshToken);
        Task<bool> SaveRefreshTokenAsync(User user, string refreshToken);
    }

}
