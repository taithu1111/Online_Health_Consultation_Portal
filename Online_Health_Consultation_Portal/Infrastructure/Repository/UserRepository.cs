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
                    .FirstOrDefaultAsync(d => d.UserId == userId);
            }
            else if (user.Role == "Patient")
            {
                var patient = await context.Patients
                    .FirstOrDefaultAsync(p => p.UserId == userId);
            }

            return user;
        }

        public async Task<bool> UpdateUserProfileAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return true;
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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var normalizedEmail = email.ToUpper(); // or use UserManager.NormalizeEmail(email)
            // Console.WriteLine($"Looking for normalized email: {normalizedEmail}");
            // return await context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
            
            return await context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }
    }
}