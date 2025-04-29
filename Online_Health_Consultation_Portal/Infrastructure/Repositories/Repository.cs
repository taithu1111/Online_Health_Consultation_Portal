using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Online_Health_Consultation_Portal.Infrastructure.Repositories
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
    {
        public async Task<T?> GetByIdAsync(int id) 
            => await context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() 
            => await context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) 
            => await context.Set<T>().Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) 
            => await context.Set<T>().AddAsync(entity);

        public Task Update(T entity)
        {
            context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}