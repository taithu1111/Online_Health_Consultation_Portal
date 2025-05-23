using Online_Health_Consultation_Portal.Domain.Entities;

namespace Online_Health_Consultation_Portal.Infrastructure.Repository
{
    public interface ILogRepository : IRepository<Log>
    {
        Task<IEnumerable<Log>> GetLogsByUserIdAsync(string userId);
        Task<IEnumerable<Log>> GetLogsByEntityAsync(string entity, int entityId);
    }
}
