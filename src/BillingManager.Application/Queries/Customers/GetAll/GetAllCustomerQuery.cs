using BillingManager.Domain.Utils;
using MediatR;

namespace BillingManager.Application.Queries.Customers.GetAll;

/// <summary>
/// Get all customers query (with pagination)
/// </summary>
public sealed class GetAllCustomerQuery : PaginationParameters, IRequest<PagedList<CustomerQueryResponse>>
{
}