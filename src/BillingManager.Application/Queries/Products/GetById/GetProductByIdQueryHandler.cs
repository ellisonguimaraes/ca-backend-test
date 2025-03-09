using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Queries.Products.GetById;

/// <summary>
/// Get product by id handler
/// </summary>
public class GetProductByIdQueryHandler(IRepository<Product> productRepository, IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductQueryResponse>
{
    public async Task<ProductQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await productRepository.GetByIdAsync(request.Id);
        return mapper.Map<ProductQueryResponse>(customer);
    }
}