using Online_Health_Consultation_Portal.Application.Dtos.Paginated;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserWithProfileAsync(int userId);
        Task<bool> UpdateUserProfileAsync(User user);
        Task<PaginatedResponse<User>> GetPaginatedUsersAsync(
            int page,
            int pageSize,
            string? roleFilter,
            string? searchTerm
        );
        Task<User?> GetUserByEmailAsync(string email);
    }
}