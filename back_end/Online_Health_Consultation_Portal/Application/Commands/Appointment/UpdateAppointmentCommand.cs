using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;


namespace Online_Health_Consultation_Portal.Application.Command.Appointment
{
    public class UpdateAppointmentCommand : IRequest<bool>
    {
        public int AppointmentId { get; set; }
        public UpdateAppointmentDto Appointment { get; set; }
    }
}
