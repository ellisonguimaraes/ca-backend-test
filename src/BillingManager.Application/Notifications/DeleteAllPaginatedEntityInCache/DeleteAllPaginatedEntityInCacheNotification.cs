using BillingManager.Domain.Entities;
using MediatR;

namespace BillingManager.Application.Notifications.DeleteAllPaginatedEntityInCache;

/// <summary>
/// Notification to delete paginated data cache in DistributedCache.
/// </summary>
public class DeleteAllPaginatedEntityInCacheNotification<TEntity> : INotification where TEntity : BaseEntity
{
}