namespace Online_Health_Consultation_Portal.Application.Dtos.Appointment
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Type { get; set; }
        public string? Notes { get; set; }
    }
}
