using AutoMapper;
using BillingManager.Application.Notifications.DeleteAllPaginatedEntityInCache;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Products.Update;

/// <summary>
/// Update product command handler
/// </summary>
public class UpdateProductCommandHandler(
    IRepository<Product> productRepository, 
    IMapper mapper,
    IMediator mediator) : IRequestHandler<UpdateProductCommand, ProductCommandResponse>
{
    public async Task<ProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        
        product = await productRepository.UpdateAsync(product);
        
        await mediator.Publish(new UpdateEntityInCacheNotification<Product> { Entity = product }, cancellationToken);
        await mediator.Publish(new DeleteAllPaginatedEntityInCacheNotification<Product>(), cancellationToken);
        
        return mapper.Map<ProductCommandResponse>(product);
    }
}