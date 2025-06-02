// SpecializationService.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Specializations;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure;

namespace Online_Health_Consultation_Portal.Services
{
    public interface ISpecializationService
    {
        Task<List<SpecializationDto>> GetAllSpecializationsAsync(string filter = null);
    }

    public class SpecializationService : ISpecializationService
    {
        private readonly AppDbContext _context;

        public SpecializationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SpecializationDto>> GetAllSpecializationsAsync(string filter = null)
        {
            var query = _context.Specializations.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(s => s.Name.Contains(filter) || s.Description.Contains(filter));
            }

            return await query
                .OrderBy(s => s.Name)
                .Select(s => new SpecializationDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description
                })
                .ToListAsync();
        }
    }
}