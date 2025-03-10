using AutoMapper;
using BillingManager.Application.Notifications.DeleteAllPaginatedEntityInCache;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Customers.Create;

/// <summary>
/// Create customer command handler
/// </summary>
public class CreateCustomerCommandHandler(
    IRepository<Customer> customerRepository, 
    IMapper mapper,
    IMediator mediator) : IRequestHandler<CreateCustomerCommand, CustomerCommandResponse>
{
    public async Task<CustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request);
        
        customer = await customerRepository.CreateAsync(customer);
        
        await mediator.Publish(new UpdateEntityInCacheNotification<Customer> { Entity = customer }, cancellationToken);
        await mediator.Publish(new DeleteAllPaginatedEntityInCacheNotification<Customer>(), cancellationToken);
        
        return mapper.Map<CustomerCommandResponse>(customer);
    }
}