using BillingManager.Domain.Entities;
using MediatR;

namespace BillingManager.Application.Notifications.DeleteAllPaginatedEntityInCache;

public class DeleteAllPaginatedEntityInCacheNotification<TEntity> : INotification where TEntity : BaseEntity
{
}