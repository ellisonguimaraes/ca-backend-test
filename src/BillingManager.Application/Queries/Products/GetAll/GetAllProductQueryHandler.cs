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
        var cacheKey = $"{nameof(Product).ToLower()}:paginated:{request.PageSize}:{request.PageNumber}";
        
        var cachedPagedProducts = await cache.GetAsync<PagedList<Product>>(cacheKey);

        if (cachedPagedProducts is not null)
        {
            return new PagedList<ProductQueryResponse>(
                cachedPagedProducts.Items.Select(mapper.Map<ProductQueryResponse>), 
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
}