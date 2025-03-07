using System.Text.Json.Serialization;

namespace BillingManager.Application.Commands.Customers;

/// <summary>
/// Generic customer command response
/// </summary>
public class CustomerCommandResponse : BaseCommandResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }
}