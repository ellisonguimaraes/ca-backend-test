using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Queries.Products.GetAll;

/// <summary>
/// Get all products handler (with pagination)
/// </summary>
public class GetAllProductQueryHandler(IRepository<Product> productRepository, IMapper mapper) : IRequestHandler<GetAllProductQuery, PagedList<ProductQueryResponse>>
{
    public async Task<PagedList<ProductQueryResponse>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var pagedProducts = await productRepository.GetPaginateAsync(request.PageNumber, request.PageSize);

        var pagedGetAllProductQueryResponse = new PagedList<ProductQueryResponse>(
            pagedProducts.Select(mapper.Map<ProductQueryResponse>), 
            pagedProducts.CurrentPage, 
            pagedProducts.PageSize, 
            pagedProducts.TotalCount);

        return pagedGetAllProductQueryResponse;
    }
}