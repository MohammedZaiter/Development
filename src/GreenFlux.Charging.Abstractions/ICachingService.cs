
namespace GreenFlux.Charging.Abstractions
{
    using System.Threading.Tasks;

    /// <summary>
    /// Cache abstraction that encapsulates cache operations.
    /// </summary>
    public interface ICachingService
    {
        /// <summary>
        /// Keys the exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<bool> KeyExists(string key);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<T> Get<T>(string key);

        /// <summary>
        /// Increments the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task Increment(string key, long value);

        /// <summary>
        /// Decrements the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task Decrement(string key, long value);

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task Delete(string key);
    }
}
