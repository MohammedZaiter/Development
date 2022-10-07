
namespace Microsoft.Extensions.DependencyInjection
{
    using GreenFlux.Charging.Groups;
    using GreenFlux.Charging.Groups.Store;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<Manager>()
                .AddScoped<IGroupsManager, Manager>(sp => sp.GetRequiredService<Manager>())
                .AddScoped<IStationsManager, Manager>(sp => sp.GetRequiredService<Manager>())
                .AddScoped<IConnectorsManager, Manager>(sp => sp.GetRequiredService<Manager>())
                .AddScoped<Store>()
                .AddScoped<IGroupsStore, Store>(sp => sp.GetRequiredService<Store>())
                .AddScoped<IStationsStore, Store>(sp => sp.GetRequiredService<Store>())
                .AddScoped<IConnectorsStore, Store>(sp => sp.GetRequiredService<Store>());
        }
    }
}
