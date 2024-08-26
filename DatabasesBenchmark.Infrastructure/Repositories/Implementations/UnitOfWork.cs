using DatabasesBenchmark.Domain.Entities;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;

namespace DatabasesBenchmark.Infrastructure.Repositories.Implementations
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly TContext _context;
        private GenericRepository<Benchmark> _benchmarkRepository;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public IGenericRepository<Benchmark> BenchmarkRepository => _benchmarkRepository ??= new GenericRepository<Benchmark>(_context);

        /// <summary>
        /// Asynchronously saves all changes made in the context to the database within a transaction.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a result of the number of state entries written to the database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Disposes of the context and releases all resources used by it.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
