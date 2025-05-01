using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class GetAppointmentDetailHandler : IRequestHandler<GetAppointmentDetailQuery, AppointmentDto>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAppointmentDetailHandler(AppDbContext context, IAutoMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AppointmentDto> Handle(GetAppointmentDetailQuery request, CancellationToken cancellationToken)
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst("role")?.Value;  // Lấy role từ token
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("id")?.Value;  // Lấy ID từ token

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(x => x.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
                throw new Exception("Appointment not found.");

            if (userRole == "Admin")
            {
                // Admin có thể xem bất kỳ cuộc hẹn nào
                return _mapper.Map<AppointmentDto>(appointment);
            }
            else if (userRole == "Patient")
            {
                // Patient chỉ có thể xem cuộc hẹn của mình
                if (appointment.PatientId != int.Parse(userId))
                    throw new UnauthorizedAccessException("You can only view your own appointments.");

                return _mapper.Map<AppointmentDto>(appointment);
            }
            else if (userRole == "Doctor")
            {
                // Doctor chỉ có thể xem các cuộc hẹn của mình
                if (appointment.DoctorId != int.Parse(userId))
                    throw new UnauthorizedAccessException("You can only view your own appointments.");

                return _mapper.Map<AppointmentDto>(appointment);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have permission to view this appointment.");
            }
        }
    }
}
