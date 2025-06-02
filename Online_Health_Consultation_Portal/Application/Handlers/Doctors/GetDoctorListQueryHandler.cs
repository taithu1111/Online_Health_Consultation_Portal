using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Doctors;
using Online_Health_Consultation_Portal.Application.Dtos.Pagination;
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
                .Include(d => d.Specializations)
                .AsQueryable();

            // Apply filters
            if (request.Specializations != null && request.Specializations.Any())
            {
                if (request.StrictSpecializationFilter)
                {
                    // Strict mode: doctor must have all the specializations in the filter list

                    // For EF Core to translate, one common way is:
                    // For each specialization ID in the filter list,
                    // doctor.Specializations must contain it.

                    foreach (var specId in request.Specializations)
                    {
                        dbQuery = dbQuery.Where(d => d.Specializations.Any(s => s.Id == specId));
                    }
                }
                else
                {
                    // Normal mode: doctor must have at least one specialization in the filter list
                    dbQuery = dbQuery.Where(d => d.Specializations.Any(s => request.Specializations.Contains(s.Id)));
                }
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