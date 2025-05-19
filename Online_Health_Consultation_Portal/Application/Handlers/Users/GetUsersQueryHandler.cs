using AutoMapper;
using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;
using Online_Health_Consultation_Portal.Infrastructure.Repository;
using Online_Health_Consultation_Portal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Online_Health_Consultation_Portal.Application.Dtos.Pagination;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedResponse<UserResponse>>
{
    private readonly AppDbContext _context;

    private readonly IUserRepository _userRepository;
    private readonly IAutoMapper _mapper;
    public GetUsersQueryHandler(IUserRepository userRepository, IAutoMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<UserResponse>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedResult = await _userRepository.GetPaginatedUsersAsync(
            request.Page,
            request.PageSize,
            request.RoleFilter,
            request.SearchTerm);

        // Mapping User -> UserResponse
        var userResponses = _mapper.Map<List<UserResponse>>(paginatedResult.Items);

        return new PaginatedResponse<UserResponse>
        {
            Items = userResponses,
            TotalCount = paginatedResult.TotalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}