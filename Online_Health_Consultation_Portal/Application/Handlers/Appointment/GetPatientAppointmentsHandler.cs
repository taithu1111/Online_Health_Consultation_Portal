using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;
using Online_Health_Consultation_Portal.Application.Queries.Appointment;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Appointment
{
    public class GetPatientAppointmentsHandler : IRequestHandler<GetPatientAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetPatientAppointmentsHandler(AppDbContext context, IAutoMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor; // lấy thông tin người dùng từ HttpContext
        }
        public async Task<List<AppointmentDto>> Handle(GetPatientAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst("role")?.Value; // lấy role từ token
            if (userRole == "Patient")
            {
                var patientId = _httpContextAccessor.HttpContext.User.FindFirst("id")?.Value; // lấy id từ token
                var appointments = await _context.Appointments
                    .Where(a => a.PatientId == int.Parse(patientId)) // truy vấn cuộc hẹn của bệnh nhân
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<AppointmentDto>>(appointments);
            }
            else
            {
                throw new UnauthorizedAccessException("You do not have access!");
            }
        }
    }
}