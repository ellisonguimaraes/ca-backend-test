using AutoMapper;
using BillingManager.Application.Notifications.UpdatePaginateEntityInCache;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;
using BillingManager.Infra.Data.Repositories.Interfaces;
using BillingManager.Services.Interfaces;
using MediatR;

namespace BillingManager.Application.Queries.Products.GetAll;

/// <summary>
/// Get all products handler (with pagination)
/// </summary>
public class GetAllProductQueryHandler(
    IRepository<Product> productRepository, 
    ICachingService cache, 
    IMediator mediator,
    IMapper mapper) : IRequestHandler<GetAllProductQuery, PagedList<ProductQueryResponse>>
{
    public async Task<PagedList<ProductQueryResponse>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        if (TryGetCachePagedProducts(request.PageSize, request.PageNumber, out var cachedPagedProducts))
        {
            return new PagedList<ProductQueryResponse>(
                cachedPagedProducts!.Items.Select(mapper.Map<ProductQueryResponse>), 
                cachedPagedProducts.CurrentPage, 
                cachedPagedProducts.PageSize, 
                cachedPagedProducts.TotalCount);
        }
        
        var pagedProducts = await productRepository.GetPaginateAsync(request.PageNumber, request.PageSize);
        
        await mediator.Publish(new UpdatePaginateEntityInCacheNotification<PagedList<Product>, Product> { PagedEntities = pagedProducts }, cancellationToken);
        
        var pagedGetAllProductQueryResponse = new PagedList<ProductQueryResponse>(
            pagedProducts.Items.Select(mapper.Map<ProductQueryResponse>), 
            pagedProducts.CurrentPage, 
            pagedProducts.PageSize, 
            pagedProducts.TotalCount);

        return pagedGetAllProductQueryResponse;
    }
    
    /// <summary>
    /// Try to get paginated product in Distributed Cache
    /// </summary>
    /// <param name="pageSize">Page size</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="products">Products</param>
    /// <returns>Get or not boolean</returns>
    private bool TryGetCachePagedProducts(int pageSize, int pageNumber, out PagedList<Product>? products)
    {
        var cacheKey = $"{nameof(Product).ToLower()}:paginated:{pageSize}:{pageNumber}";
        products = cache.GetAsync<PagedList<Product>>(cacheKey).Result;
        return products is not null;
    }
}