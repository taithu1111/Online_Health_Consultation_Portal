using MediatR;
using Online_Health_Consultation_Portal.Application.Commands.ConsultationSession;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.ConsultationSession
{
    public class StartConsultationSessionHandler : IRequestHandler<StartConsultationSessionCommand, bool>
    {
        private readonly AppDbContext _context;
        public StartConsultationSessionHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(StartConsultationSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _context.ConsultationSessions.FindAsync(request.Id);
            if (session == null)
            {
                return false;
            }
            session.StartTime = DateTime.UtcNow; // set thời gian bắt đầu
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
