using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;
using MediatR;

namespace BillingManager.Application.Notifications.UpdatePaginateEntityInCache;

public class UpdatePaginateEntityInCacheNotification<TPagedEntity, TEntity> : INotification 
    where TPagedEntity : PagedList<TEntity> 
    where TEntity : BaseEntity
{
    public TPagedEntity PagedEntities { get; set; }
}