using BillingManager.Domain.Entities;
using BillingManager.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace BillingManager.Application.Notifications.UpdateEntityInCache;

/// <summary>
/// Handler to update entity data cache in DistributedCache.
/// </summary>
/// <param name="cache">Cache service</param>
/// <param name="configuration">Configuration file (appsettings)</param>
public class UpdateEntityInCacheNotificationHandler<T>(ICachingService cache, IConfiguration configuration) : INotificationHandler<UpdateEntityInCacheNotification<T>> where T : BaseEntity
{
    #region Constants
    private const string CACHING_EXPIRATION_TIME_IN_MINUTES_CONFIG_NAME = "CachingExpirationTimeInMinutes";
    #endregion
    
    private readonly int _expirationTimeInMinutes = int.Parse(configuration[CACHING_EXPIRATION_TIME_IN_MINUTES_CONFIG_NAME]!);
    
    public async Task Handle(UpdateEntityInCacheNotification<T> notification, CancellationToken cancellationToken)
    {
        var cacheKey = $"{typeof(T).Name.ToLower()}:{notification.Entity.Id}";
        await cache.SetAsync(cacheKey, notification.Entity, TimeSpan.FromMinutes(_expirationTimeInMinutes));
    }
}