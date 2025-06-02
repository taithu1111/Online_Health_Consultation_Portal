namespace Online_Health_Consultation_Portal.Application.Dtos.Doctors
{
    public class DoctorListRequestDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<int>? Specializations { get; set; }  // changed from List<string>
        public int? MinExperienceYears { get; set; }
        public string? Language { get; set; }
        public bool StrictSpecializationFilter { get; set; } = false; // default normal mode
    }
}