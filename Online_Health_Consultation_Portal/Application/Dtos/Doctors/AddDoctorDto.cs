namespace Online_Health_Consultation_Portal.Application.Dtos.Doctors
{
    public class AddDoctorDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public IFormFile? ProfileImage { get; set; }

        public int ExperienceYears { get; set; }
        public string Languages { get; set; }
        public string Bio { get; set; }
        public decimal ConsultationFee { get; set; }
        public List<int>? SpecializationIds { get; set; }
    }
}