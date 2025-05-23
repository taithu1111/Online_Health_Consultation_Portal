using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Schedule;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Schedule
{
    public class UpdateScheduleHandler : IRequestHandler<UpdateScheduleCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        public UpdateScheduleHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = _context.Schedules
                .FirstOrDefault(x => x.Id == request.Id);
            if (schedule == null)
            {
                return false;
            }
            
            _mapper.Map<Domain.Entities.Schedule>(request); // Map từ UpdateScheduleCommand sang entity Schedule
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
