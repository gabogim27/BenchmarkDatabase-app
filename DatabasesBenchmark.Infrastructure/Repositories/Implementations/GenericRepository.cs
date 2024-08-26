using DatabasesBenchmark.Infrastructure.DbContext;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabasesBenchmark.Infrastructure.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Microsoft.EntityFrameworkCore.DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(Microsoft.EntityFrameworkCore.DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            else
            {
                await _dbSet.AddAsync(entity);
            }
        }

        /// <summary>
        /// Asynchronously retrieves an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified identifier, or null if no such entity is found.</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Asynchronously updates the specified entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            else
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
    }

}
