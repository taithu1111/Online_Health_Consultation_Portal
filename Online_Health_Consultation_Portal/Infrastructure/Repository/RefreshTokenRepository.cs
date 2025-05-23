using Microsoft.AspNetCore.Identity;
using Online_Health_Consultation_Portal.Application.Interfaces.Repository;
using Online_Health_Consultation_Portal.Domain.Entities;

namespace Online_Health_Consultation_Portal.Infrastructure.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly UserManager<User> _userManager;

        public RefreshTokenRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> ValidateRefreshTokenAsync(User user, string refreshToken)
        {
            return user.RefreshToken == refreshToken &&
                   user.RefreshTokenExpiryTime > DateTime.UtcNow;
        }

        public async Task<bool> SaveRefreshTokenAsync(User user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }

}
