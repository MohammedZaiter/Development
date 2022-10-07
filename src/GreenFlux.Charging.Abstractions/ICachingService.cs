
namespace GreenFlux.Charging.Abstractions
{
    using System.Threading.Tasks;

    public interface ICachingService
    {
        Task<bool> KeyExists(string key);

        Task<T> Get<T>(string key);

        Task Increment(string key, long value);

        Task Decrement(string key, long value);

        Task Delete(string key);
    }
}
