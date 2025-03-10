using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;
using BillingManager.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace BillingManager.Application.Notifications.UpdatePaginateEntityInCache;

public class UpdatePaginateEntityInCacheNotificationHandler<TEntity>(ICachingService cache, IConfiguration configuration)
    : INotificationHandler<UpdatePaginateEntityInCacheNotification<PagedList<TEntity>, TEntity>> 
    where TEntity : BaseEntity
{
    #region Constants
    private const string CACHING_EXPIRATION_TIME_IN_MINUTES_CONFIG_NAME = "CachingExpirationTimeInMinutes";
    #endregion
    
    private readonly int _expirationTimeInMinutes = int.Parse(configuration[CACHING_EXPIRATION_TIME_IN_MINUTES_CONFIG_NAME]!);
    
    public async Task Handle(UpdatePaginateEntityInCacheNotification<PagedList<TEntity>, TEntity> notification, CancellationToken cancellationToken)
    {
        var cacheKey = $"{typeof(TEntity).Name.ToLower()}:paginated:{notification.PagedEntities.PageSize}:{notification.PagedEntities.CurrentPage}";
        await cache.SetAsync(cacheKey, notification.PagedEntities, TimeSpan.FromMinutes(_expirationTimeInMinutes));
    }
}