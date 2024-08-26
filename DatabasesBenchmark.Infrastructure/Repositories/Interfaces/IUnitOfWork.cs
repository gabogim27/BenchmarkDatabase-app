using DatabasesBenchmark.Domain.Entities;

namespace DatabasesBenchmark.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Benchmark> BenchmarkRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
