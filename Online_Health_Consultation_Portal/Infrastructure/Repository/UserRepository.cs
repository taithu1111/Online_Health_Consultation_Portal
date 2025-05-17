using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Paginated;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context, UserManager<User> userManager) : IUserRepository
    {
        public async Task<User?> GetUserWithProfileAsync(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            if (user.Role == "Doctor")
            {
                var doctor = await context.Doctors
                    .Include(d => d.Specialization)
                    .FirstOrDefaultAsync(d => d.Id == userId);
            }
            else if (user.Role == "Patient")
            {
                var patient = await context.Patients
                    .FirstOrDefaultAsync(p => p.UserId == userId);
            }

            return user;
        }

        public async Task UpdateUserProfileAsync(User user)
        {
            await userManager.UpdateAsync(user);
        }

        public async Task<PaginatedResponse<User>> GetPaginatedUsersAsync(
            int page, 
            int pageSize, 
            string? roleFilter, 
            string? searchTerm)
        {
            var query = context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                query = query.Where(u => u.Role == roleFilter);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u =>
                    u.FullName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<User>
            {
                Items = users,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}