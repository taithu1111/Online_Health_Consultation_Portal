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
        public CreateAppointmentHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = _mapper.Map<Domain.Entities.Appointment, CreateAppointmentCommand>(request);
            appointment.Status = "Pending";  // Mặc định là Pending

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            return appointment.Id;  // Trả về ID cuộc hẹn
        }
    }
}
