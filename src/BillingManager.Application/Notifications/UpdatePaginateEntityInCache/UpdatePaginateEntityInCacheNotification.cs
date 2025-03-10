using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;
using MediatR;

namespace BillingManager.Application.Notifications.UpdatePaginateEntityInCache;

/// <summary>
/// Notification to update paginated entity data cache in DistributedCache.
/// </summary>
public class UpdatePaginateEntityInCacheNotification<TPagedEntity, TEntity> : INotification 
    where TPagedEntity : PagedList<TEntity> 
    where TEntity : BaseEntity
{
    public TPagedEntity PagedEntities { get; set; }
}