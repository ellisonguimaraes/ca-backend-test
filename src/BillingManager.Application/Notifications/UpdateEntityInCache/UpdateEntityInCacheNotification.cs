using BillingManager.Domain.Entities;
using MediatR;

namespace BillingManager.Application.Notifications.UpdateEntityInCache;

/// <summary>
/// Notification to update entity data cache in DistributedCache.
/// </summary>
public sealed class UpdateEntityInCacheNotification<TEntity> : INotification where TEntity : BaseEntity
{
    public TEntity Entity { get; set; }
}