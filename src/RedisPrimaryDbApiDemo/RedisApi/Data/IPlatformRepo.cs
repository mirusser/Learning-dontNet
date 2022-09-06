using RedisApi.Models;

namespace RedisApi.Data;

public interface IPlatformRepo
{
    void Create(Platform platform);

    Platform? Get(string id);

    IEnumerable<Platform?> GetAll();
}