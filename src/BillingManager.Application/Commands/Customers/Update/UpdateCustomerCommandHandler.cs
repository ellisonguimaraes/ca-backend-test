using AutoMapper;
using BillingManager.Application.Commands.Customers.Create;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Customers.Update;

/// <summary>
/// Update customer command handler
/// </summary>
public class UpdateCustomerCommandHandler(IRepository<Customer> customerRepository, IMapper mapper) : IRequestHandler<CreateCustomerCommand, CustomerCommandResponse>
{
    public async Task<CustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request);
        customer = await customerRepository.UpdateAsync(customer);
        return mapper.Map<CustomerCommandResponse>(customer);
    }
}