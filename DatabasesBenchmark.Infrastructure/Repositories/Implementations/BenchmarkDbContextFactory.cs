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

        /// <summary>
        /// Creates an instance of <see cref="IUnitOfWork"/> based on the specified database provider.
        /// </summary>
        /// <param name="provider">The database provider to use for creating the unit of work.</param>
        /// <returns>An instance of <see cref="IUnitOfWork"/> configured for the specified database provider.</returns>
        /// <exception cref="NotSupportedException">Thrown when the specified database provider is not supported.</exception>
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

        /// <summary>
        /// Creates an instance of <see cref="MySqlBenchmarkDbContext"/> configured for MySQL database access.
        /// </summary>
        /// <returns>An instance of <see cref="MySqlBenchmarkDbContext"/>.</returns>
        private MySqlBenchmarkDbContext CreateMySqlDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlBenchmarkDbContext>();
            optionsBuilder.UseMySql(_configuration.GetConnectionString("MySQLConnection"),
                new MySqlServerVersion(new Version(8, 0, 23)));

            return new MySqlBenchmarkDbContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Creates an instance of <see cref="PostgreBenchmarkDbContext"/> configured for PostgreSQL database access.
        /// </summary>
        /// <returns>An instance of <see cref="PostgreBenchmarkDbContext"/>.</returns>
        private PostgreBenchmarkDbContext CreatePostgresDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreBenchmarkDbContext>();
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgresConnection"));

            return new PostgreBenchmarkDbContext(optionsBuilder.Options);
        }
    }
}
