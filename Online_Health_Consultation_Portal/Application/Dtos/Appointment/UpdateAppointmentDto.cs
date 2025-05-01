namespace Online_Health_Consultation_Portal.Application.Dtos.Appointment
{
    public class UpdateAppointmentDto
    {
        public DateTime AppointmentDateTime { get; set; }
        public string Type { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; }
    }
}
