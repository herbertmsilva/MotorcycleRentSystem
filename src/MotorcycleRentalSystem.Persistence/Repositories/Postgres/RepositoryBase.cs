using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MotorcycleRentalSystem.Persistence.Context;
using MotorcycleRentalSystem.Core.Entities;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Persistence.Repositories.Postgres
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
            => _context = context;

        public async Task AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
            => await _context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(Guid id, List<string> includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            var entity = await query.FirstOrDefaultAsync(q=>q.Id.Equals(id));
            return entity!;
        }

        public async Task<List<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().Where(predicate).ToListAsync();

    }
}
