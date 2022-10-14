
namespace GreenSystem.Charging.Caching.Redis
{
    using GreenSystem.Charging.Abstractions;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register caching services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static IServiceCollection AddCaching(this IServiceCollection services, string connectionString)
        {
            return services
                .AddScoped<ICachingService, RedisCache>(sp => new RedisCache(connectionString));
        }
    }
}
