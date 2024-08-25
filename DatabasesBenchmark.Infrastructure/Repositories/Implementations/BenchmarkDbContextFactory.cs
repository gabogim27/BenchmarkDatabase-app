using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Infrastructure.DbContext;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatabasesBenchmark.Infrastructure.Repositories.Implementations
{
    public class BenchmarkDbContextFactory : IBenchmarkDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public BenchmarkDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IUnitOfWork CreateUnitOfWork(DatabaseProvider provider)
        {
            switch (provider)
            {
                case DatabaseProvider.MySQL:
                    var mySqlContext = CreateMySqlDbContext();
                    return new UnitOfWork<MySqlBenchmarkDbContext>(mySqlContext);

                case DatabaseProvider.PostgreSQL:
                    var postgresContext = CreatePostgresDbContext();
                    return new UnitOfWork<PostgreBenchmarkDbContext>(postgresContext);

                default:
                    throw new NotSupportedException($"Provider {provider} is not supported.");
            }
        }

        private MySqlBenchmarkDbContext CreateMySqlDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlBenchmarkDbContext>();
            optionsBuilder.UseMySql(_configuration.GetConnectionString("MySQLConnection"),
                new MySqlServerVersion(new Version(8, 0, 23)));

            return new MySqlBenchmarkDbContext(optionsBuilder.Options);
        }

        private PostgreBenchmarkDbContext CreatePostgresDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreBenchmarkDbContext>();
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQLConnection"));

            return new PostgreBenchmarkDbContext(optionsBuilder.Options);
        }
    }


}
