using MotorcycleRentalSystem.Core.Entities;
using System.Linq.Expressions;

namespace MotorcycleRentalSystem.Core.Interfaces
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByIdAsync(Guid id, List<string> includes);
        Task<List<T?>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}