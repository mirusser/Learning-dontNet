using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;

namespace RedisConfigurationDemo
{
    internal class Program
    {
        private static IDistributedCache _cache = new RedisCache(
                Options.Create(new RedisCacheOptions()
                {
                    Configuration = "127.0.0.1:6379"
                }));

        public static void Main(string[] args)
        {
            //AbsoluteExpiration();

            //SlidingExpiration();

            var foo = _cache.GetString("key");
        }

        public static void AbsoluteExpiration()
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20)
            };

            _cache.SetString("key", "string", options);
        }

        public static void SlidingExpiration()
        {
            var options = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(30)
            };

            _cache.SetString("key", "string", options);
        }
    }
}