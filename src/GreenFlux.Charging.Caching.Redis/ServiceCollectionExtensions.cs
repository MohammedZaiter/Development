
namespace GreenFlux.Charging.Caching.Redis
{
    using GreenFlux.Charging.Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, string connectionString)
        {
            return services
                .AddScoped<ICachingService, RedisCache>(sp => new RedisCache(connectionString));
        }
    }
}
