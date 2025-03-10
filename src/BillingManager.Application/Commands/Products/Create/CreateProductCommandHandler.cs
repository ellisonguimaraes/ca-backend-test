using AutoMapper;
using BillingManager.Application.Notifications.DeleteAllPaginatedEntityInCache;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Products.Create;

/// <summary>
/// Create product command handler
/// </summary>
public class CreateProductCommandHandler(
    IRepository<Product> productRepository, 
    IMapper mapper,
    IMediator mediator) : IRequestHandler<CreateProductCommand, ProductCommandResponse>
{
    public async Task<ProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);

        product = await productRepository.CreateAsync(product);
        
        await mediator.Publish(new UpdateEntityInCacheNotification<Product> { Entity = product }, cancellationToken);
        await mediator.Publish(new DeleteAllPaginatedEntityInCacheNotification<Product>(), cancellationToken);
        
        return mapper.Map<ProductCommandResponse>(product);
    }
}