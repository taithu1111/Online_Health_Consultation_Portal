using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Pagination;
using Online_Health_Consultation_Portal.Application.Dtos.Users;

namespace Online_Health_Consultation_Portal.Application.Queries.Users
{
    public sealed record GetUsersQuery : IRequest<PaginatedResponse<UserWithProfile>>
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public string? RoleFilter { get; init; }
        public string? SearchTerm { get; init; }
    }
}