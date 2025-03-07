using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Queries.Customers.GetById;

/// <summary>
/// Get customer by id query
/// </summary>
public sealed class GetCustomerByIdQuery : IRequest<CustomerQueryResponse>
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}