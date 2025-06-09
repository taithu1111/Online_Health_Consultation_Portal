namespace Online_Health_Consultation_Portal.Application.Dtos.Doctors
{
    public class DoctorListRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? SpecializationId { get; set; }
        public int? MinExperienceYears { get; set; }
        public string? Language { get; set; }
    }
}