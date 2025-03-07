using System.Text.Json.Serialization;

namespace BillingManager.Application.Queries.Customers;

/// <summary>
/// Generic customer query response
/// </summary>
public class CustomerQueryResponse : BaseQueryResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }
}
