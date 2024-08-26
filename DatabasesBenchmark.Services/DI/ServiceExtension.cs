using DatabasesBenchmark.Services.Helpers.Implementations;
using DatabasesBenchmark.Services.Helpers.Interfaces;
using DatabasesBenchmark.Services.Implementations;
using DatabasesBenchmark.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabasesBenchmark.Services.DI
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServicesToDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBenchmarkService, BenchmarkService>();
            services.AddScoped<IStringHelper, StringHelper>();
            return services;
        }
    }
}
