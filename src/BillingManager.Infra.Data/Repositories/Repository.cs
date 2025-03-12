using System.Linq.Expressions;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.Resources;
using BillingManager.Domain.Utils;
using BillingManager.Infra.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BillingManager.Infra.Data.Repositories;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context = context;
    
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        => await DbSet.SingleOrDefaultAsync(e => e.Id.Equals(id));

    public virtual Task<PagedList<TEntity>> GetPaginateAsync(int pageNumber, int pageSize)
        => Task.FromResult(new PagedList<TEntity>(
                DbSet.OrderBy(i => i.Id),
                pageNumber,
                pageSize
            ));

    public virtual Task<PagedList<TEntity>> GetPaginateAsync<TKey>(
        int pageNumber, 
        int pageSize, 
        Expression<Func<TEntity, TKey>> orderByProperty,
        Expression<Func<TEntity, bool>>? expression = null)
        => BuildPagedList(DbSet, pageNumber, pageSize, orderByProperty, expression);

    /// <summary>
    /// Build Paged List
    /// </summary>
    /// <param name="queryable">IQueryable entity data</param>
    /// <param name="parameters">Page number and page size</param>
    /// <param name="orderByProperty">Order by property (lambda expression)</param>
    /// <param name="expression">Filter expression (lambda expression)</param>
    /// <returns>Returns paged, filtered and sorted entity</returns>
    protected Task<PagedList<TEntity>> BuildPagedList<TKey>(
        IQueryable<TEntity> queryable,
        int pageNumber, 
        int pageSize,
        Expression<Func<TEntity, TKey>> orderByProperty,
        Expression<Func<TEntity, bool>>? expression = null)
        => Task.FromResult(new PagedList<TEntity>(
            expression is null
                ? queryable
                    .OrderBy(orderByProperty)
                : queryable
                    .Where(expression)
                    .OrderBy(orderByProperty),
            pageNumber,
            pageSize
        ));

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = entity.CreatedAt;
        
        DbSet.Add(entity);
        await Context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var dbEntity = await DbSet.SingleOrDefaultAsync(e => e.Id.Equals(entity.Id))
                       ?? throw new BusinessException(ErrorsResource.NOT_FOUND_ERROR_CODE, string.Format(ErrorsResource.NOT_FOUND_ERROR_MESSAGE, typeof(TEntity).Name));

        entity.UpdatedAt = dbEntity.UpdatedAt = DateTime.UtcNow;
        
        Context.Entry(dbEntity).CurrentValues.SetValues(entity);
        Context.Entry(dbEntity).Property(e => e.CreatedAt).IsModified = false;

        await Context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<TEntity> DeleteAsync(Guid id)
    {
        var entity = await DbSet.SingleOrDefaultAsync(e => e.Id.Equals(id)) 
                     ?? throw new BusinessException(ErrorsResource.NOT_FOUND_ERROR_CODE, string.Format(ErrorsResource.NOT_FOUND_ERROR_MESSAGE, typeof(TEntity).Name));

        DbSet.Remove(entity);
        await Context.SaveChangesAsync();

        return entity;
    }
}