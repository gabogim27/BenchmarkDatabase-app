using DatabasesBenchmark.Domain.Enums;

namespace DatabasesBenchmark.Infrastructure.Repositories.Interfaces
{
    public interface IBenchmarkDbContextFactory
    {
        IUnitOfWork CreateUnitOfWork(DatabaseProvider provider);
    }
}
