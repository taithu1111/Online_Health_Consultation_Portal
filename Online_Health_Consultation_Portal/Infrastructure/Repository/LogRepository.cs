using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Domain;
using System;

namespace Online_Health_Consultation_Portal.Infrastructure.Repository
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        private AppDbContext _context;

        public LogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Log>> GetLogsByUserIdAsync(string userId)
        {
            return await _context.Logs
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetLogsByEntityAsync(string entity, int entityId)
        {
            return await _context.Logs
                .Where(l => l.Entity == entity && l.EntityId == entityId)
                .ToListAsync();
        }
    }
}
