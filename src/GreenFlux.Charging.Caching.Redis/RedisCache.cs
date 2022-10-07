
namespace GreenFlux.Charging.Caching.Redis
{
    using GreenFlux.Charging.Abstractions;
    using StackExchange.Redis;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;

    public sealed class RedisCache : ICachingService
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;
        private readonly string instance;

        public RedisCache(string connectionString)
        {
            this.connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        }

        public async Task<bool> KeyExists(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var db = this.GetDatabase();

            return await db.KeyExistsAsync($"{instance}:{key}");
        }

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

        public async Task Increment(string key, long value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var db = this.GetDatabase();

            await db.StringIncrementAsync(key, value);
        }

        public async Task Decrement(string key, long value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var db = this.GetDatabase();

            await db.StringDecrementAsync(key, value);
        }

        public Task Delete(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var db = this.GetDatabase();

            return db.KeyDeleteAsync(key, CommandFlags.FireAndForget);
        }

        private IDatabase GetDatabase()
        {
            return this.connectionMultiplexer.GetDatabase();
        }
    }
}