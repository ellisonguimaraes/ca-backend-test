using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Customers.Update;

/// <summary>
/// Update customer command handler
/// </summary>
public class UpdateCustomerCommandHandler(IRepository<Customer> customerRepository, IMapper mapper) : IRequestHandler<UpdateCustomerCommand, CustomerCommandResponse>
{
    public async Task<CustomerCommandResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request);
        customer = await customerRepository.UpdateAsync(customer);
        return mapper.Map<CustomerCommandResponse>(customer);
    }
}