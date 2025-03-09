using BillingManager.Domain.Utils;
using MediatR;

namespace BillingManager.Application.Queries.Products.GetAll;

/// <summary>
/// Get all products query (with pagination)
/// </summary>
public sealed class GetAllProductQuery : PaginationParameters, IRequest<PagedList<ProductQueryResponse>>
{
}