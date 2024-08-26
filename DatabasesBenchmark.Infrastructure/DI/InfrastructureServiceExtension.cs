using DatabasesBenchmark.Infrastructure.DbContext;
using DatabasesBenchmark.Infrastructure.Repositories.Implementations;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabasesBenchmark.Infrastructure.DI
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructureServicesToDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MySqlBenchmarkDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("MySQLConnection"), new MySqlServerVersion(new Version(8, 0, 23))));
            services.AddDbContext<PostgreBenchmarkDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
            services.AddScoped<IBenchmarkDbContextFactory, BenchmarkDbContextFactory>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddUnitOfWork<MySqlBenchmarkDbContext>();
            services.AddUnitOfWork<PostgreBenchmarkDbContext>();

            return services;
        }

        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : Microsoft.EntityFrameworkCore.DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return services;
        }
    }
}
