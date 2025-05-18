using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Infrastructure.Service
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task LogInformationAsync(string message, string userId, string action, string entity, int entityId)
        {
            var log = new Log
            {
                Message = message,
                Level = "Information",
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                Action = action,
                Entity = entity,
                EntityId = entityId
            };

            await _logRepository.AddAsync(log);
        }

        public async Task LogWarningAsync(string message, string? userId, string action, string entity, int entityId)
        {
            var log = new Log
            {
                Message = message,
                Level = "Warning",
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                Action = action,
                Entity = entity,
                EntityId = entityId
            };

            await _logRepository.AddAsync(log);
        }

        public async Task LogErrorAsync(string message, string userId, string action, string entity, int entityId)
        {
            var log = new Log
            {
                Message = message,
                Level = "Error",
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                Action = action,
                Entity = entity,
                EntityId = entityId
            };

            await _logRepository.AddAsync(log);
        }
    }
}
