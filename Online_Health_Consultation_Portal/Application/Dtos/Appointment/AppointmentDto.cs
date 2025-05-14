namespace Online_Health_Consultation_Portal.Application.Dtos.Appointment
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        //public int PatientId { get; set; }
        //public int DoctorId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string? Notes { get; set; }
        public string? Diagnosis { get; set; } // chuẩn đoán
    }
}
