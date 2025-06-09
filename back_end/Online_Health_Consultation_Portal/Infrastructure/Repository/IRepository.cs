using Online_Health_Consultation_Portal.Domain;

namespace Online_Health_Consultation_Portal.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> Get();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
