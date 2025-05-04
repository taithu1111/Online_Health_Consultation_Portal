using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Commands.Schedule;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Schedule
{
    public class DeleteScheduleHandler : IRequestHandler<DeleteScheduleCommand, bool>
    {
        private readonly AppDbContext _context;
        public DeleteScheduleHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (schedule == null)
            {
                return false;
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
