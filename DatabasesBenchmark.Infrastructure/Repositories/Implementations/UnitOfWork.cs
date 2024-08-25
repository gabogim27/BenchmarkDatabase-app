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

        public IGenericRepository<Benchmark> BenchmarkRepository
        {
            get => _benchmarkRepository ??= new GenericRepository<Benchmark>(_context);
        }

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

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
