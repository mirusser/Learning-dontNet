using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RandomQuotesApi.Services.Contracts;

namespace RandomQuotesApi.Services;

public class CacheService(IDistributedCache cache) : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    
    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var bytes = await cache.GetAsync(key, ct);
        if (bytes is null || bytes.Length == 0)
            return default;

        return JsonSerializer.Deserialize<T>(bytes, JsonOptions);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? ttl = null,
        CancellationToken ct = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);

        var options = new DistributedCacheEntryOptions();
        if (ttl.HasValue)
            options.SetAbsoluteExpiration(ttl.Value);

        await cache.SetAsync(key, bytes, options, ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
        => await cache.RemoveAsync(key, ct);

    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> valueFactory,
        TimeSpan? ttl = null,
        CancellationToken ct = default)
    {
        var existing = await GetAsync<T>(key, ct);
        if (!Equals(existing, default(T)) && existing is not null)
            return existing;

        var value = await valueFactory(ct);
        await SetAsync(key, value, ttl, ct);
        return value;
    }
}