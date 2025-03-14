using AutoMapper;
using BillingManager.Application.Notifications.UpdatePaginateEntityInCache;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;
using BillingManager.Infra.Data.Repositories.Interfaces;
using BillingManager.Services.Interfaces;
using MediatR;

namespace BillingManager.Application.Queries.Customers.GetAll;

/// <summary>
/// Get all customers handler (with pagination)
/// </summary>
public class GetAllCustomerQueryHandler(
    IRepository<Customer> customerRepository, 
    ICachingService cache, 
    IMediator mediator,
    IMapper mapper) : IRequestHandler<GetAllCustomerQuery, PagedList<CustomerQueryResponse>>
{
    public async Task<PagedList<CustomerQueryResponse>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
    {
        if (TryGetCachePagedCustomer(request.PageSize, request.PageNumber, out var cachedPagedCustomers))
        {
            return new PagedList<CustomerQueryResponse>(
                cachedPagedCustomers!.Items.Select(mapper.Map<CustomerQueryResponse>), 
                cachedPagedCustomers.CurrentPage, 
                cachedPagedCustomers.PageSize, 
                cachedPagedCustomers.TotalCount);
        }
        
        var pagedCustomers = await customerRepository.GetPaginateAsync(request.PageNumber, request.PageSize);
        
        await mediator.Publish(new UpdatePaginateEntityInCacheNotification<PagedList<Customer>, Customer> { PagedEntities = pagedCustomers }, cancellationToken);
        
        var pagedGetAllCustomerQueryResponse = new PagedList<CustomerQueryResponse>(
            pagedCustomers.Items.Select(mapper.Map<CustomerQueryResponse>), 
            pagedCustomers.CurrentPage, 
            pagedCustomers.PageSize, 
            pagedCustomers.TotalCount);

        return pagedGetAllCustomerQueryResponse;
    }

    /// <summary>
    /// Try to get paginated customer in Distributed Cache
    /// </summary>
    /// <param name="pageSize">Page size</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="customers">Customers</param>
    /// <returns>Get or not boolean</returns>
    private bool TryGetCachePagedCustomer(int pageSize, int pageNumber, out PagedList<Customer>? customers)
    {
        var cacheKey = $"{nameof(Customer).ToLower()}:paginated:{pageSize}:{pageNumber}";
        customers = cache.GetAsync<PagedList<Customer>>(cacheKey).Result;
        return customers is not null;
    }
}