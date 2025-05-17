namespace Online_Health_Consultation_Portal.Application.Dtos.Doctors
{
    public class DoctorDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public string Languages { get; set; }
        public string Bio { get; set; }
        public decimal ConsultationFee { get; set; }
        public double AverageRating { get; set; }
    }
}