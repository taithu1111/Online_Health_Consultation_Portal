using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateAppointmentHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst("role")?.Value;  // Lấy role từ token
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("id")?.Value;  // Lấy ID từ token

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            if (userRole == "Admin")
            {
                // Admin có thể cập nhật bất kỳ cuộc hẹn nào
            }
            else if (userRole == "Patient")
            {
                // Patient chỉ có thể cập nhật cuộc hẹn của chính mình
                if (appointment.PatientId != int.Parse(userId))
                    throw new UnauthorizedAccessException("You can only update your own appointments.");

                // Cập nhật thông tin
                appointment.AppointmentDateTime = request.Appointment.AppointmentDateTime;
                appointment.Status = request.Appointment.Status;
                appointment.Type = request.Appointment.Type;
                appointment.Notes = request.Appointment.Notes;
            }
            else if (userRole == "Doctor")
            {
                // Doctor chỉ có thể cập nhật các cuộc hẹn của mình
                if (appointment.DoctorId != int.Parse(userId))
                    throw new UnauthorizedAccessException("You can only update your own appointments.");

                // Cập nhật thông tin
                appointment.AppointmentDateTime = request.Appointment.AppointmentDateTime;
                appointment.Status = request.Appointment.Status;
                appointment.Type = request.Appointment.Type;
                appointment.Notes = request.Appointment.Notes;
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have access to update this appointment.");
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
