using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Commands.Customers.Update;

/// <summary>
/// Update customer command
/// </summary>
public sealed class UpdateCustomerCommand : IRequest<CustomerCommandResponse>
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }
}
