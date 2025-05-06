using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.Schedule;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Schedule
{
    public class CreateScheduleHandler : IRequestHandler<CreateScheduleCommand, int>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        public CreateScheduleHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Domain.Schedule, CreateScheduleCommand>(request); // Map từ CreateScheduleCommand sang  entity Schedule
            _context.Schedules.Add(schedule);


            await _context.SaveChangesAsync(cancellationToken);
            return schedule.Id;
        }
    }
}
