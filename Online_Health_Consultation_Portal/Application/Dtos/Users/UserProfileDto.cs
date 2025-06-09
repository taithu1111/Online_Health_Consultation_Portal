namespace Online_Health_Consultation_Portal.Application.Dtos.Users
{
    public class UserProfileDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }

        // Patient specific fields
        public string ImageUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        
        // Doctor specific fields
        public string Bio { get; set; }
        public string Specialization { get; set; }
        public int? ExperienceYears { get; set; }
        public string Languages { get; set; }
        public decimal? ConsultationFee { get; set; }
    }
}