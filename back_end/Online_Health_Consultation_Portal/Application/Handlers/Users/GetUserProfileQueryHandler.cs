using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Users;
using Online_Health_Consultation_Portal.Application.Queries.Users;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Users
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetUserProfileQueryHandler(
            UserManager<User> userManager,
            AppDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserProfileDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(request.User);
            if (user == null)
            {
                return null;  // Let controller handle null case
            }

            var role = user.Role;

            Doctor? doctor = null;
            Patient? patient = null;

            if (role == "Doctor")
            {
                doctor = await _context.Doctors
                    .Include(d => d.Specialization)
                    .FirstOrDefaultAsync(d => d.UserId == user.Id, cancellationToken);
            }
            else if (role == "Patient")
            {
                patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
            }

            var wrapper = new UserWithProfile
            {
                User = user,
                Doctor = doctor,
                Patient = patient
            };

            var dto = _mapper.Map<UserProfileDto>(wrapper);
            
            if (role != null)
            {
                dto.Role = role;
            }

            return dto;
        }
    }
}