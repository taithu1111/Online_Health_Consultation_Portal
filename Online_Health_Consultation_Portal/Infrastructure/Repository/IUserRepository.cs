using Online_Health_Consultation_Portal.Application.Dtos.Pagination;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserWithProfileAsync(int userId);
        Task UpdateUserProfileAsync(User user);
        Task<PaginatedResponse<User>> GetPaginatedUsersAsync(
            int page, 
            int pageSize, 
            string? roleFilter, 
            string? searchTerm
        );
        Task<User?>GetByIdAsync(int userId);
    }
}