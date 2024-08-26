using DatabasesBenchmark.Domain.Entities;
using DatabasesBenchmark.Infrastructure.DbContext;
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
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
