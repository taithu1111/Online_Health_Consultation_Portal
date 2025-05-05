using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, bool>
    {
        private readonly AppDbContext _context;
        public CancelAppointmentHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
        }
        public async Task<bool> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            appointment.Status = "Cancelled";
            await _context.SaveChangesAsync(cancellationToken);

            return true;  // Trả về true nếu hủy thành công
        }
    }
}