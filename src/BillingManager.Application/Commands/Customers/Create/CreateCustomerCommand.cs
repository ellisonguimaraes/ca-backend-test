using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Commands.Customers.Create;

/// <summary>
/// Create customer command
/// </summary>
public sealed class CreateCustomerCommand : IRequest<CustomerCommandResponse>
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }
}
