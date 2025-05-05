using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;
using Online_Health_Consultation_Portal.Application.Queries.Schedule;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Schedule
{
    public class GetDoctorSchedulesQueryHandler :  IRequestHandler<GetDoctorSchedulesQuery, List<ScheduleDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        public GetDoctorSchedulesQueryHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ScheduleDto>> Handle(GetDoctorSchedulesQuery request, CancellationToken cancellationToken)
        {
            //var schedules = await _context.Schedules
            //    .Where(s => s.DoctorId == request.DoctorId)
            //    .ToListAsync(cancellationToken);

            //return _mapper.Map<List<ScheduleDto>>(schedules);
            var schedules = await _context.Schedules
                .Where(s => s.DoctorId == request.DoctorId)
                .Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    DoctorId = s.DoctorId,
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsAvailable = s.IsAvailable
                })
                .ToListAsync(cancellationToken);
            return schedules;
        }
    }
}
