using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, bool>
    {
        private readonly AppDbContext _context;
        public UpdateAppointmentHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            appointment.AppointmentDateTime = request.Appointment.AppointmentDateTime;
            appointment.Status = request.Appointment.Status;
            appointment.Type = request.Appointment.Type;
            appointment.Notes = request.Appointment.Notes;

            await _context.SaveChangesAsync(cancellationToken);
            return true;  // Trả về true nếu cập nhật thành công
        }
    }
}
