using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DatabasesBenchmark.Infrastructure.DbContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlBenchmarkDbContext>, IDesignTimeDbContextFactory<PostgreBenchmarkDbContext>
    {
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

        PostgreBenchmarkDbContext IDesignTimeDbContextFactory<PostgreBenchmarkDbContext>.CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DatabasesBenchmark.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PostgreBenchmarkDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSQLConnection"),
                    options => options.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromMilliseconds(100),
                        errorCodesToAdd: null));

            return new PostgreBenchmarkDbContext(optionsBuilder.Options);
        }
    }
}
