using AutoMapper;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.Resources;
using BillingManager.Infra.Data.Repositories.Interfaces;
using BillingManager.Services.Interfaces;
using MediatR;

namespace BillingManager.Application.Queries.Customers.GetById;

/// <summary>
/// Get customer by id handler
/// </summary>
public class GetCustomerByIdQueryHandler(
    IRepository<Customer> customerRepository, 
    ICachingService cache, 
    IMediator mediator,
    IMapper mapper) : IRequestHandler<GetCustomerByIdQuery, CustomerQueryResponse>
{
    public async Task<CustomerQueryResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        if (TryGetCachedCustomer(request.Id, out var cachedCustomer))
            return mapper.Map<CustomerQueryResponse>(cachedCustomer);
        
        var customer = await customerRepository.GetByIdAsync(request.Id)
                       ?? throw new BusinessException(ErrorsResource.NOT_FOUND_ERROR_CODE, string.Format(ErrorsResource.NOT_FOUND_ERROR_MESSAGE, nameof(Customer)));

        await mediator.Publish(new UpdateEntityInCacheNotification<Customer> { Entity = customer }, cancellationToken);
        
        return mapper.Map<CustomerQueryResponse>(customer);
    }
    
    /// <summary>
    /// Try to get customer in Distributed Cache
    /// </summary>
    /// <param name="id">Customer identifier</param>
    /// <param name="customer">Customer</param>
    /// <returns>Get or not boolean</returns>
    private bool TryGetCachedCustomer(Guid id, out Customer? customer)
    {
        var cacheKey = $"{nameof(Customer).ToLower()}:{id}";
        customer = cache.GetAsync<Customer>(cacheKey).Result;
        return customer is not null;
    }
}