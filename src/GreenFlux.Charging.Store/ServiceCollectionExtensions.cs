
namespace Microsoft.Extensions.DependencyInjection
{
    using GreenFlux.Charging.Store;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStore(this IServiceCollection services, DataStoreOptions options)
        {
            return services
                .AddSingleton(options)
                .AddScoped<IConnectionManager, ConnectionManager>();
        }
    }
}
