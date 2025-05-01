using MediatR;

namespace Online_Health_Consultation_Portal.Application.Command.Appointment
{
    public class CancelAppointmentCommand : IRequest
    {
        public int AppointmentId { get; set; }
    }
}