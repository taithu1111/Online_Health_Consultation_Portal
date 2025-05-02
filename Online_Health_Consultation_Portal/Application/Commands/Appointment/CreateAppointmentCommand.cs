using MediatR;
using Online_Health_Consultation_Portal.Application.Dtos.Appointment;

namespace Online_Health_Consultation_Portal.Application.Command.Appointment
{
    public class CreateAppointmentCommand : IRequest<int>
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Type { get; set; }
        public string? Notes { get; set; }
    }
}
