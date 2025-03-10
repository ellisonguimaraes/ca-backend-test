using System.Linq.Expressions;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;

namespace BillingManager.Infra.Data.Repositories.Interfaces;

/// <summary>
/// Generic repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="id">Guid identifier</param>
    /// <returns>Entity or null</returns>
    Task<TEntity> GetByIdAsync(Guid id);

    /// <summary>
    /// Get paged entity list
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Entity paged list</returns>
    Task<PagedList<TEntity>> GetPaginateAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Get paged entity list (with filter and order by property)
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="orderByProperty">Order by property (lambda expression)</param>
    /// <param name="expression">Filter expression (lambda expression)</param>
    /// <returns>Returns paged, filtered and sorted entity</returns>
    Task<PagedList<TEntity>> GetPaginateAsync<TKey>(
        int pageNumber, 
        int pageSize,
        Expression<Func<TEntity, TKey>> orderByProperty,
        Expression<Func<TEntity, bool>>? expression = null);

    /// <summary>
    /// Create entity in database
    /// </summary>
    /// <param name="entity">New entity</param>
    /// <returns>New registered entity with identifier (guid)</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Update entity in database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <returns>Updated entity</returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Delete entity in database
    /// </summary>
    /// <param name="id">Identifier (guid)</param>
    /// <returns>Deleted entity</returns>
    Task<TEntity> DeleteAsync(Guid id);
}