namespace BillingManager.Services.Interfaces;

/// <summary>
/// Distributed cache service
/// </summary>
public interface ICachingService
{
    /// <summary>
    /// Get key/value in distributed cache
    /// </summary>
    /// <param name="key">Key to access</param>
    /// <returns>Deserialized value</returns>
    Task<T?> GetAsync<T>(string key);
    
    /// <summary>
    /// Set key/value in distributed cache
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <param name="expiry">Expiration time</param>
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

    /// <summary>
    /// Delete key by prefix
    /// </summary>
    /// <param name="prefix">Prefix name</param>
    Task DeleteAsync(string prefix);
}