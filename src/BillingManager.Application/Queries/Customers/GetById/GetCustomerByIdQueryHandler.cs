using AutoMapper;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Domain.Entities;
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
        var cacheKey = $"{nameof(Customer).ToLower()}:{request.Id}";
        
        var cachedCustomer = await cache.GetAsync<Customer>(cacheKey);

        if (cachedCustomer is not null)
        {
            return mapper.Map<CustomerQueryResponse>(cachedCustomer);
        }
        
        var customer = await customerRepository.GetByIdAsync(request.Id);

        await mediator.Publish(new UpdateEntityInCacheNotification<Customer> { Entity = customer }, cancellationToken);
        
        return mapper.Map<CustomerQueryResponse>(customer);
    }
}