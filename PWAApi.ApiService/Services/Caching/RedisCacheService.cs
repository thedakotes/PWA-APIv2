
using System.Text.Json;
using StackExchange.Redis;

namespace PWAApi.ApiService.Services.Caching
{
    public class RedisCacheService : ICacheService
    {
        /*
         * To see what's in your redis cache, you need to open a command line window on your machine where Docker is running
         * 
         * If you already know the container ID of your redis cache, you can skip this part.
         * 
         *  > docker ps
         *  
         * This will get you your list of redis container IDs.
         * 
         * Then:
         * 
         *  > docker exec -it [containerIDnobrackets] redis-cli
         *  
         *  This opens the redis command line for your container. The next commands are executable within the redis-cli.
         * 
         * To get a list of your keys:
         * 
         *  > keys *
         * 
         * To look at a specific key:
         *  
         *  > get [keyname]
         *  
         * */
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiry);
        }
    }
}
