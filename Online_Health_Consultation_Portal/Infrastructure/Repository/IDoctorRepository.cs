using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetFilteredDoctorsAsync(int? specializationId, int? minExperience, string? language);
        Task<Doctor?> GetByIdAsync(int userId);
    }
}