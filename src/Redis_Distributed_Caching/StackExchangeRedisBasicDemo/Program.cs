using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;

namespace StackExchangeRedisBasicDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = Options.Create(new RedisCacheOptions()
            {
                Configuration = "127.0.0.1"
            });

            IDistributedCache cache = new RedisCache(options);

            cache.SetString("rat", "bob");
            Console.WriteLine(cache.GetString("rat"));

            Console.ReadKey();
        }

        public static void RawStackExchangeRedis()
        {
            //var redis = ConnectionMultiplexer.Connect("127.0.0.1");
            //var db = redis.GetDatabase();

            //Console.WriteLine(db.StringGet("dog"));
            //Console.WriteLine(db.StringGet("cat"));
        }
    }
}