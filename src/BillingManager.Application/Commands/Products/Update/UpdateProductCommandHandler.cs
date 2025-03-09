using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Products.Update;

/// <summary>
/// Update product command handler
/// </summary>
public class UpdateProductCommandHandler(IRepository<Product> productRepository, IMapper mapper) : IRequestHandler<UpdateProductCommand, ProductCommandResponse>
{
    public async Task<ProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        product = await productRepository.UpdateAsync(product);
        return mapper.Map<ProductCommandResponse>(product);
    }
}