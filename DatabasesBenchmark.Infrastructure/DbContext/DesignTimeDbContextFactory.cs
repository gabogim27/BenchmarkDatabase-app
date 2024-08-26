using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DatabasesBenchmark.Infrastructure.DbContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlBenchmarkDbContext>, IDesignTimeDbContextFactory<PostgreBenchmarkDbContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MySqlBenchmarkDbContext"/> with configuration settings read from the appsettings.json file.
        /// </summary>
        /// <param name="args">Command line arguments passed to the factory.</param>
        /// <returns>A new instance of <see cref="MySqlBenchmarkDbContext"/>.</returns>
        public MySqlBenchmarkDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DatabasesBenchmark.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MySqlBenchmarkDbContext>();
            optionsBuilder.UseMySql(configuration.GetConnectionString("MySQLConnection"),
                new MySqlServerVersion(new Version(8, 0, 23)));

            return new MySqlBenchmarkDbContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Creates a new instance of <see cref="PostgreBenchmarkDbContext"/> with configuration settings read from the appsettings.json file.
        /// </summary>
        /// <param name="args">Command line arguments passed to the factory.</param>
        /// <returns>A new instance of <see cref="PostgreBenchmarkDbContext"/>.</returns>
        PostgreBenchmarkDbContext IDesignTimeDbContextFactory<PostgreBenchmarkDbContext>.CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DatabasesBenchmark.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PostgreBenchmarkDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                    options => options.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromMilliseconds(100),
                        errorCodesToAdd: null));

            return new PostgreBenchmarkDbContext(optionsBuilder.Options);
        }
    }
}
