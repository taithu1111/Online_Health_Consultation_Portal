using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public class DoctorRepository(AppDbContext context) : IDoctorRepository
    {
        public async Task<List<Doctor>> GetFilteredDoctorsAsync(int? specializationId, int? minExperience, string? language)
        {
            var query = context.Doctors.Include(d => d.User).Include(d => d.Specialization).AsQueryable();
            
            //if (specializationId.HasValue)
            //    query = query.Where(d => d.SpecializationId == specializationId);
            
            if (minExperience.HasValue)
                query = query.Where(d => d.ExperienceYears >= minExperience);
            
            if (!string.IsNullOrEmpty(language))
                query = query.Where(d => d.Languages.Contains(language));
            
            return await query.ToListAsync();
        }
        
        public async Task<Doctor?> GetByIdAsync(int userId) 
            => await context.Doctors
                .Include(d => d.User)
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(d => d.Id == userId);
    }
}