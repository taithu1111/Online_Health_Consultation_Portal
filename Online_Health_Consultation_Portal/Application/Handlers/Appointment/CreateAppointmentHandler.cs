using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Command.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, int>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateAppointmentHandler(AppDbContext context, IAutoMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst("role")?.Value;  // Lấy role từ token
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("id")?.Value;  // Lấy ID từ token

            if (userRole == "Admin")
            {
                // Admin có thể tạo cuộc hẹn cho bất kỳ bệnh nhân và bác sĩ nào
                var appointment = _mapper.Map<Domain.Appointment>(request.Appointment);
                appointment.Status = "Pending";  // Mặc định cuộc hẹn là Pending

                // Đảm bảo rằng bác sĩ và bệnh nhân là hợp lệ
                var doctorExists = await _context.Doctors
                    .AnyAsync(d => d.UserId == appointment.DoctorId, cancellationToken);
                var patientExists = await _context.Patients
                    .AnyAsync(p => p.UserId == appointment.PatientId, cancellationToken);

                if (!doctorExists || !patientExists)
                    throw new Exception("Doctor or Patient not found.");

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                return appointment.Id;
            }
            else if (userRole == "Patient")
            {
                // Patient chỉ có thể tạo cuộc hẹn với bác sĩ mà họ đã chọn
                var appointment = _mapper.Map<Domain.Appointment>(request.Appointment);
                appointment.PatientId = int.Parse(userId);  // Gán ID bệnh nhân từ token
                appointment.Status = "Pending";  // Mặc định cuộc hẹn là Pending

                // Đảm bảo rằng bác sĩ là hợp lệ
                var doctorExists = await _context.Doctors
                    .AnyAsync(d => d.UserId == appointment.DoctorId, cancellationToken);
                if (!doctorExists)
                    throw new Exception("Doctor not found.");

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync(cancellationToken);

                return appointment.Id;
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have access to create appointments.");
            }
        }
    }
}
