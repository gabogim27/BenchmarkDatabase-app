using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Infrastructure.DbContext;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DatabasesBenchmark.Infrastructure.Repositories.Implementations
{
    public class BenchmarkDbContextFactory : IBenchmarkDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BenchmarkDbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork CreateUnitOfWork(DatabaseProvider provider)
        {
            Microsoft.EntityFrameworkCore.DbContext context = provider switch
            {
                DatabaseProvider.MySQL => ActivatorUtilities.CreateInstance<MySqlBenchmarkDbContext>(_serviceProvider),
                DatabaseProvider.PostgreSQL => ActivatorUtilities.CreateInstance<PostgreBenchmarkDbContext>(_serviceProvider),
                _ => throw new ArgumentException("Invalid database provider")
            };

            // Asegúrate de que UnitOfWork reciba una instancia de DbContext
            return new UnitOfWork<Microsoft.EntityFrameworkCore.DbContext>(context);
        }
    }
}
