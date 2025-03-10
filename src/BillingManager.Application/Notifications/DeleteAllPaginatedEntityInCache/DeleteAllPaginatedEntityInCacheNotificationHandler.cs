using BillingManager.Domain.Entities;
using BillingManager.Services.Interfaces;
using MediatR;

namespace BillingManager.Application.Notifications.DeleteAllPaginatedEntityInCache;

/// <summary>
/// Handler to delete paginated data cache in DistributedCache.
/// </summary>
/// <param name="cache">Cache service</param>
public class DeleteAllPaginatedEntityInCacheNotificationHandler<TEntity>(ICachingService cache) : INotificationHandler<DeleteAllPaginatedEntityInCacheNotification<TEntity>> where TEntity : BaseEntity
{
    public async Task Handle(DeleteAllPaginatedEntityInCacheNotification<TEntity> notification, CancellationToken cancellationToken)
    {
        var cachePrefix = $"{typeof(TEntity).Name.ToLower()}:paginated:";
        await cache.DeleteAsync(cachePrefix);
    }
}