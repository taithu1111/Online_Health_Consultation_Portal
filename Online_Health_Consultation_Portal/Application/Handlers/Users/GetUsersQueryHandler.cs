using AutoMapper;
using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Application.Dtos.Pagination;
using Online_Health_Consultation_Portal.Domain;
using Microsoft.EntityFrameworkCore;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedResponse<UserWithProfile>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper, AppDbContext context)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedResponse<UserWithProfile>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedResult = await _userRepository.GetPaginatedUsersAsync(
            request.Page,
            request.PageSize,
            request.RoleFilter,
            request.SearchTerm);

        var userWithProfiles = new List<UserWithProfile>();

        foreach (var user in paginatedResult.Items)
        {
            Doctor? doctor = null;
            Patient? patient = null;

            if (user.Role == "Doctor")
            {
                doctor = await _context.Doctors
                    .Include(d => d.Specializations)
                    .FirstOrDefaultAsync(d => d.UserId == user.Id);
            }
            else if (user.Role == "Patient")
            {
                patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);
            }

            userWithProfiles.Add(new UserWithProfile
            {
                User = user,
                Doctor = doctor,
                Patient = patient
            });
        }

        return new PaginatedResponse<UserWithProfile>
        {
            Items = userWithProfiles,
            TotalCount = paginatedResult.TotalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
