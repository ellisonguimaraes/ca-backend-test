using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Products.Create;

/// <summary>
/// Create product command handler
/// </summary>
public class CreateProductCommandHandler(IRepository<Product> productRepository, IMapper mapper) : IRequestHandler<CreateProductCommand, ProductCommandResponse>
{
    public async Task<ProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        product = await productRepository.CreateAsync(product);
        return mapper.Map<ProductCommandResponse>(product);
    }
}