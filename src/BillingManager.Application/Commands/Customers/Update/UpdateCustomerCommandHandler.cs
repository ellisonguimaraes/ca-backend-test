using AutoMapper;
using BillingManager.Application.Notifications.DeleteAllPaginatedEntityInCache;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Customers.Update;

/// <summary>
/// Update customer command handler
/// </summary>
public class UpdateCustomerCommandHandler(
    IRepository<Customer> customerRepository, 
    IMapper mapper,
    IMediator mediator) : IRequestHandler<UpdateCustomerCommand, CustomerCommandResponse>
{
    public async Task<CustomerCommandResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request);
        
        customer = await customerRepository.UpdateAsync(customer);
        
        await mediator.Publish(new UpdateEntityInCacheNotification<Customer> { Entity = customer }, cancellationToken);
        await mediator.Publish(new DeleteAllPaginatedEntityInCacheNotification<Customer>(), cancellationToken);
        
        return mapper.Map<CustomerCommandResponse>(customer);
    }
}