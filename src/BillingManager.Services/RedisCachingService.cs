using System.Text.Json;
using BillingManager.Domain.Configurations;
using BillingManager.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace BillingManager.Services;

public sealed class RedisCachingService(
    IDistributedCache cache,
    RedisSettings redisSettings) : ICachingService
{
    private readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect(redisSettings.Host);
    
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await cache.GetStringAsync(key);
        return value is null ? default! : JsonSerializer.Deserialize<T>(value)!;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serializedObj = JsonSerializer.Serialize(value);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry
        };
            
        await cache.SetStringAsync(key, serializedObj, options);
    }

    public async Task DeleteAsync(string prefix)
    {
        var keys = GetKeysByPrefix(prefix);
        
        if (keys.Any())
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(keys.Select(k => new RedisKey(k)).ToArray());
        }
    }
    
    /// <summary>
    /// Get redis keys by prefix
    /// </summary>
    /// <param name="prefix">Prefix name</param>
    /// <returns>All keys with prefix</returns>
    private IList<string> GetKeysByPrefix(string prefix)
    {
        var server = _redis.GetServer(_redis.GetEndPoints().First());
        var keys = server.Keys(pattern: $"{redisSettings.InstanceName}{prefix}*").ToList();
        
        return keys.Select(k => k.ToString()).ToList();
    }
}