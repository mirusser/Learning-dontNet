using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Settings;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Core.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableMediatrQuery
    {
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _settings;

        public CachingBehavior(
            IDistributedCache cache, 
            IOptions<CacheSettings> settings)
        {
            _cache = cache;
            _settings = settings.Value;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is ICacheableMediatrQuery cacheableQuery)
            {
                TResponse response;

                if (cacheableQuery.BypassCache) return await next();

                async Task<TResponse> GetResponseAndAddToCache() //TODO: move outside?
                {
                    response = await next();

                    var slidingExpiration = cacheableQuery.SlidingExpiration ?? TimeSpan.FromHours(_settings.SlidingExpiration);
                    var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
                    var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));

                    await _cache.SetAsync(cacheableQuery.CacheKey, serializedData, options, cancellationToken);

                    return response;
                }

                var cachedResponse = await _cache.GetAsync(cacheableQuery.CacheKey, cancellationToken);

                if (cachedResponse != null)
                {
                    response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
                }
                else
                {
                    response = await GetResponseAndAddToCache();
                }

                return response;
            }

            return await next();
        }
    }
}
