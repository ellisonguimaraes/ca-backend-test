using AutoMapper;
using BillingManager.Application.Commands.Customers;
using BillingManager.Application.Commands.Customers.Create;
using BillingManager.Application.Commands.Customers.Update;
using BillingManager.Application.Queries.Customers;
using BillingManager.Domain.Entities;

namespace BillingManager.Application.Profiles;

/// <summary>
/// AutoMapper profile for customer
/// </summary>
public sealed class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<UpdateCustomerCommand, Customer>();
        CreateMap<Customer, CustomerCommandResponse>();
        CreateMap<Customer, CustomerQueryResponse>();
    }
}
