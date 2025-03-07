using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Queries.Customers.GetAll;

/// <summary>
/// Get all customers handler (with pagination)
/// </summary>
public class GetAllCustomerQueryHandler(IRepository<Customer> customerRepository, IMapper mapper) : IRequestHandler<GetAllCustomerQuery, PagedList<CustomerQueryResponse>>
{
    public async Task<PagedList<CustomerQueryResponse>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        var pagedCustomers = await customerRepository.GetPaginateAsync(request.PageNumber, request.PageSize);

        var pagedGetAllCustomerQueryResponse = new PagedList<CustomerQueryResponse>(
            pagedCustomers.Select(mapper.Map<CustomerQueryResponse>), 
            pagedCustomers.CurrentPage, 
            pagedCustomers.PageSize, 
            pagedCustomers.TotalCount);

        return pagedGetAllCustomerQueryResponse;
    }
}