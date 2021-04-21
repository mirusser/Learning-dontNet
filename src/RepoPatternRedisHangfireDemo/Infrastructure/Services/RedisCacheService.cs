using Core.Configurations;
using Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly CacheConfiguration _cacheConfig;
        private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions;

        public RedisCacheService(
            IDistributedCache distributedCache,
             IOptions<CacheConfiguration> cacheConfig)
        {
            _distributedCache = distributedCache;
            _cacheConfig = cacheConfig.Value;

            _distributedCacheEntryOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours))
                .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes));
        }

        public void Remove(string cacheKey)
            => _distributedCache.Remove(cacheKey);

        public T Set<T>(string cacheKey, T value)
        {
            var valueByteArray = JsonSerializer.SerializeToUtf8Bytes(value);
            _distributedCache.Set(cacheKey, valueByteArray, _distributedCacheEntryOptions);

            return value;
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            var valueByteArray = _distributedCache.Get(cacheKey);
            value = default;

            if (valueByteArray != null)
            {
                var serializedValue = Encoding.UTF8.GetString(valueByteArray);
                value = JsonSerializer.Deserialize<T>(serializedValue);
            }

            return valueByteArray != null;
        }
    }
}
