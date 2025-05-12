using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Service
{
    public interface IMessageRepository
    {
        Task<Message> GetByIdAsync(int id);
        Task<List<Message>> GetAllAsync();
        Task<List<Message>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id);
        Task<List<Message>> GetMessagesByUserAsync(int userId);
        Task<List<Message>> GetUnreadMessagesForUserAsync(int userId);
        Task<int> AddAsync(Message message);
        Task UpdateAsync(Message message);
        Task DeleteAsync(int id);
        Task MarkAsReadAsync(int messageId);
    }


}

