
namespace GreenFlux.Charging.Caching.Redis
{
    using GreenFlux.Charging.Abstractions;
    using StackExchange.Redis;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// Redis cache class that encapsulates cache operations.
    /// </summary>
    public sealed class RedisCache : ICachingService
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public RedisCache(string connectionString)
        {
            this.connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public async Task<T> Get<T>(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var db = this.GetDatabase();

            var redisValue = await db.StringGetAsync(key);

            if (redisValue.IsNull)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(await db.StringGetAsync(key));
        }

        /// <summary>
        /// Increments the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public async Task Increment(string key, long value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var db = this.GetDatabase();

            await db.StringIncrementAsync(key, value);
        }

        /// <summary>
        /// Decrements the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public async Task Decrement(string key, long value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var db = this.GetDatabase();

            await db.StringDecrementAsync(key, value);
        }

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">key</exception>
        public Task Delete(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var db = this.GetDatabase();

            return db.KeyDeleteAsync(key, CommandFlags.FireAndForget);
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns></returns>
        private IDatabase GetDatabase()
        {
            return this.connectionMultiplexer.GetDatabase();
        }
    }
}