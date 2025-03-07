using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Resources;
using BillingManager.Infra.Data.Repositories.Interfaces;
using MediatR;

namespace BillingManager.Application.Queries.Customers.GetById;

/// <summary>
/// Get customer by id handler
/// </summary>
public class GetCustomerByIdQueryHandler(IRepository<Customer> customerRepository, IMapper mapper) : IRequestHandler<GetCustomerByIdQuery, CustomerQueryResponse>
{
    public async Task<CustomerQueryResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.Id) 
                       ?? throw new ArgumentException($"[{ErrorsResource.NOT_FOUND_ERROR_CODE}] {string.Format(ErrorsResource.NOT_FOUND_ERROR_MESSAGE, nameof(Customer))}");

        return mapper.Map<CustomerQueryResponse>(customer);
    }
}