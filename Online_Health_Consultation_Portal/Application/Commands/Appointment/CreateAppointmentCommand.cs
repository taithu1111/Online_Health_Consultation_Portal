using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;

namespace Online_Health_Consultation_Portal.Application.Command.Appointment
{
    public class CreateAppointmentCommand : IRequest<int>
    {
        public CreateAppointmentDto Appointment { get; set; }
    }
}
