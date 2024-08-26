using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Infrastructure.DbContext;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
<<<<<<< HEAD
using Microsoft.Extensions.DependencyInjection;
=======
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8

namespace DatabasesBenchmark.Infrastructure.Repositories.Implementations
{
    public class BenchmarkDbContextFactory : IBenchmarkDbContextFactory
    {
<<<<<<< HEAD
        private readonly IServiceProvider _serviceProvider;

        public BenchmarkDbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
=======
        private readonly IConfiguration _configuration;

        public BenchmarkDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
        }

        public IUnitOfWork CreateUnitOfWork(DatabaseProvider provider)
        {
<<<<<<< HEAD
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
=======
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


>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
}
