using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Application.Queries.Doctors;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Doctors
{
    public class GetDoctorListQueryHandler : IRequestHandler<GetDoctorListQuery, List<DoctorDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetDoctorListQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            var doctors = await query.ToListAsync(cancellationToken);

            var doctorDtos = _mapper.Map<List<DoctorDto>>(doctors);

            return doctorDtos;
        }
    }
}