using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context, UserManager<User> userManager) : IUserRepository
    {
        public async Task<User?> GetUserWithProfileAsync(int userId)
            => await context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .ThenInclude(d => d.Specialization)
                .FirstOrDefaultAsync(u => u.Id == userId);

        public async Task UpdateUserProfileAsync(User user)
        {
            await userManager.UpdateAsync(user);
        }
    }
}