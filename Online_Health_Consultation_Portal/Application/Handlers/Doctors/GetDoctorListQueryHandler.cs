using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Application.Queries.Doctors;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Doctors
{
    public class GetDoctorListQueryHandler : IRequestHandler<GetDoctorListQuery, List<DoctorDto>>
    {
        private readonly AppDbContext _context;

        public GetDoctorListQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorDto>> Handle(GetDoctorListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialization)
                .AsQueryable();

            // Apply filters
            if (request.SpecializationId.HasValue)
            {
                query = query.Where(d => d.SpecializationId == request.SpecializationId);
            }

            if (request.MinExperienceYears.HasValue)
            {
                query = query.Where(d => d.ExperienceYears >= request.MinExperienceYears);
            }

            if (!string.IsNullOrEmpty(request.Language))
            {
                query = query.Where(d => d.Languages.Contains(request.Language));
            }

            var doctors = await query
                .Select(d => new DoctorDto
                {
                    UserId = d.UserId,
                    FullName = d.User.FullName,
                    Email = d.User.Email,
                    Specialization = d.Specialization.Name,
                    ExperienceYears = d.ExperienceYears,
                    Languages = d.Languages,
                    Bio = d.Bio,
                    ConsultationFee = d.ConsultationFee,
                    AverageRating = d.AverageRating
                })
                .ToListAsync(cancellationToken);

            return doctors;
        }
    }
}