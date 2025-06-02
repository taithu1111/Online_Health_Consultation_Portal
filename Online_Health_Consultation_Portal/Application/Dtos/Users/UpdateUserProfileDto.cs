namespace Online_Health_Consultation_Portal.Application.Dtos.Users
{
    public class UpdateUserProfileDto
    {
        public string? FullName { get; set; }
        public string? Gender { get; set; } // Đề phòng, chỉ cho admin quyền sửa
        public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
        
        // Patient specific fields
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? BloodType { get; set; }
        
        // Doctor specific fields
        public string? Bio { get; set; }
        public string? Languages { get; set; }
        public List<string>? Specializations { get; set; }
        public int? ExperienceYears { get; set; }
        public decimal? ConsultationFee { get; set; }
    }
}