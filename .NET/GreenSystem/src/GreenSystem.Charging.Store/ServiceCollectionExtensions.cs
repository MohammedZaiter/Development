
namespace Microsoft.Extensions.DependencyInjection
{
    using GreenSystem.Charging.Store;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register store services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static IServiceCollection AddStore(this IServiceCollection services, DataStoreOptions options)
        {
            return services
                .AddSingleton(options)
                .AddScoped<IConnectionManager, ConnectionManager>();
        }
    }
}
