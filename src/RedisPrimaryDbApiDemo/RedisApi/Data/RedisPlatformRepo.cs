using System.Text.Json;
using RedisApi.Models;
using StackExchange.Redis;

namespace RedisApi.Data;

public class RedisPlatformRepo : IPlatformRepo
{
    private readonly IDatabase db;

    private const string platformSetName = "PlatformSet";
    private const string hashKeyName = "hashplatform";

    public RedisPlatformRepo(IConnectionMultiplexer connectionMultiplexer)
    {
        db = connectionMultiplexer.GetDatabase();
    }

    public void Create(Platform platform)
    {
        if (platform is null)
        {
            throw new ArgumentOutOfRangeException(nameof(platform));
        }

        var serializedPlatform = JsonSerializer.Serialize(platform);

        db.HashSet(hashKeyName, new HashEntry[]
        {
            new HashEntry(platform.Id, serializedPlatform)
        });
    }

    public Platform? Get(string id)
    {
        var platform = db.HashGet(hashKeyName, id);

        if (!string.IsNullOrEmpty(platform))
        {
            return JsonSerializer.Deserialize<Platform>(platform);
        }

        return null;
    }

    public IEnumerable<Platform?> GetAll()
    {
        var completeHash = db.HashGetAll(hashKeyName);

        if (completeHash is not null && completeHash.Length > 0)
        {
            var platforms = Array
                .ConvertAll(
                    completeHash,
                    val => JsonSerializer.Deserialize<Platform?>(val.Value))
                .ToList();

            return platforms;
        }

        return Array.Empty<Platform?>();
    }
}