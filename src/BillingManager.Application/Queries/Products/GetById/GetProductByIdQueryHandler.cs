using AutoMapper;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.Resources;
using BillingManager.Infra.Data.Repositories.Interfaces;
using BillingManager.Services.Interfaces;
using MediatR;

namespace BillingManager.Application.Queries.Products.GetById;

/// <summary>
/// Get product by id handler
/// </summary>
public class GetProductByIdQueryHandler(
    IRepository<Product> productRepository,
    ICachingService cache, 
    IMediator mediator,
    IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductQueryResponse>
{
    public async Task<ProductQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        if (TryGetCachedProduct(request.Id, out var cachedProduct))
            return mapper.Map<ProductQueryResponse>(cachedProduct);
        
        var product = await productRepository.GetByIdAsync(request.Id)
                      ?? throw new BusinessException(ErrorsResource.NOT_FOUND_ERROR_CODE, string.Format(ErrorsResource.NOT_FOUND_ERROR_MESSAGE, nameof(Product)));
        
        await mediator.Publish(new UpdateEntityInCacheNotification<Product> { Entity = product }, cancellationToken);
        
        return mapper.Map<ProductQueryResponse>(product);
    }
    
    /// <summary>
    /// Try to get product in Distributed Cache
    /// </summary>
    /// <param name="id">Product identifier</param>
    /// <param name="product">Product</param>
    /// <returns>Get or not boolean</returns>
    private bool TryGetCachedProduct(Guid id, out Product? product)
    {
        var cacheKey = $"{nameof(Product).ToLower()}:{id}";
        product = cache.GetAsync<Product>(cacheKey).Result;
        return product is not null;
    }
}