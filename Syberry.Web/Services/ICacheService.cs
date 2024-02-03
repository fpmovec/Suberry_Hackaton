namespace Syberry.Web.Services;

public interface ICacheService
{ 
    Task<T?> GetByKeyAsync<T>(string key, CancellationToken token = default);
    Task UpdateOrCreateAsync<T>(string key, T value, CancellationToken token = default);
    Task RemoveFromCacheByKeyAsync(string key);
}