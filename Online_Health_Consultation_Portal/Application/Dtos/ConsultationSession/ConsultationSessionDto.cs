namespace Online_Health_Consultation_Portal.Application.Dtos.ConsultationSession
{
    public class ConsultationSessionDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }

        public string PatientName { get; set; }
        public string DoctorName { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string SessionNotes { get; set; }
        public string MeetingUrl { get; set; }
    }
}
