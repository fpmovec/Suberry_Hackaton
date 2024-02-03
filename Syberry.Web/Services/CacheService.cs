using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Syberry.Web.Services.Abstractions;

namespace Syberry.Web.Services;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task<T?> GetByKeyAsync<T>(string key, CancellationToken token = default)
    {
        string? jsonData = await distributedCache.GetStringAsync(key, token);

        T? data = string.IsNullOrEmpty(jsonData) ? default(T) : JsonConvert.DeserializeObject<T>(jsonData);

        return data;
    }

    public async Task UpdateOrCreateAsync<T>(string key, T value, CancellationToken token = default)
    {
        string jsonData = JsonConvert.SerializeObject(value);

        await distributedCache.SetStringAsync(key, jsonData, new DistributedCacheEntryOptions()
            {SlidingExpiration = TimeSpan.FromMinutes(15) }, default);
    }

    public async Task RemoveFromCacheByKeyAsync(string key)
    {
        await distributedCache.RemoveAsync(key, default);
    }
}