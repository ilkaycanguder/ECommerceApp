using ECommerceApp.Product.Application.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ECommerceApp.Product.Infrastructure.Services.Cache
{
    /// <summary>
    /// Redis Cache Service — Cache-Aside Pattern implementasyonu.
    /// 
    /// Dependency Inversion Principle: ICacheService interface'i Application katmanında
    /// tanımlanmıştır. Infrastructure bu interface'i implement eder.
    /// Application katmanı Redis'i bilmez, sadece interface'i kullanır.
    /// 
    /// Single Responsibility: Sadece cache işlemlerinden sorumludur.
    /// RemoveByPrefixAsync ile toplu cache invalidation desteklenir.
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var value = await _database.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return default;

            return JsonConvert.DeserializeObject<T>(value!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var serialized = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key, serialized, expiration ?? TimeSpan.FromMinutes(5));
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
            => await _database.KeyDeleteAsync(key);

        public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{prefix}*").ToArray();

            if (keys.Length > 0)
                await _database.KeyDeleteAsync(keys);
        }
    }
}
