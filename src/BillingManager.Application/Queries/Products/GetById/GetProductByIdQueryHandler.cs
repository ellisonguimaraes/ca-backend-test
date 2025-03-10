using AutoMapper;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Domain.Entities;
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
        var cacheKey = $"{nameof(Product).ToLower()}:{request.Id}";
        
        var cachedProduct = await cache.GetAsync<Product>(cacheKey);

        if (cachedProduct is not null)
        {
            return mapper.Map<ProductQueryResponse>(cachedProduct);
        }
        
        var product = await productRepository.GetByIdAsync(request.Id);
        
        await mediator.Publish(new UpdateEntityInCacheNotification<Product> { Entity = product }, cancellationToken);
        
        return mapper.Map<ProductQueryResponse>(product);
    }
}