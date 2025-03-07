using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Commands.Customers.Create;

/// <summary>
/// Create customer command handler
/// </summary>
public class CreateCustomerCommandHandler(IRepository<Customer> customerRepository, IMapper mapper) : IRequestHandler<CreateCustomerCommand, CustomerCommandResponse>
{
    public async Task<CustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = mapper.Map<Customer>(request);
        customer = await customerRepository.CreateAsync(customer);
        return mapper.Map<CustomerCommandResponse>(customer);
    }
}