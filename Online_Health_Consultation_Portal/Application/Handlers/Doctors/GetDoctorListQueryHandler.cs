using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Application.Dtos.Paginated;
using Online_Health_Consultation_Portal.Application.Queries.Doctors;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Doctors
{
    public class GetDoctorListQueryHandler : IRequestHandler<GetDoctorListQuery, PaginatedResponse<DoctorDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetDoctorListQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<DoctorDto>> Handle(GetDoctorListQuery query, CancellationToken cancellationToken)
        {
            var request = query.Request;
            var dbQuery = _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialization)
                .AsQueryable();

            // Apply filters
            if (request.SpecializationId.HasValue)
            {
                dbQuery = dbQuery.Where(d => d.SpecializationId == request.SpecializationId);
            }

            if (request.MinExperienceYears.HasValue)
            {
                dbQuery = dbQuery.Where(d => d.ExperienceYears >= request.MinExperienceYears);
            }

            if (!string.IsNullOrEmpty(request.Language))
            {
                dbQuery = dbQuery.Where(d => d.Languages.Contains(request.Language));
            }

            var totalCount = await dbQuery.CountAsync(cancellationToken);

            var paginatedDoctors = await dbQuery
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var doctorDtos = _mapper.Map<List<DoctorDto>>(paginatedDoctors);

            return new PaginatedResponse<DoctorDto>
            {
                Items = doctorDtos,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}