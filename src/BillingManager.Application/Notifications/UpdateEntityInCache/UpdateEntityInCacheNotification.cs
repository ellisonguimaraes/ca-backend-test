using BillingManager.Domain.Entities;
using MediatR;

namespace BillingManager.Application.Notifications.UpdateEntityInCache;

public sealed class UpdateEntityInCacheNotification<TEntity> : INotification where TEntity : BaseEntity
{
    public TEntity Entity { get; set; }
}