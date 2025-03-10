using System.Text.Json.Serialization;

namespace BillingManager.Application.Queries.Customers;

/// <summary>
/// Generic customer query response
/// </summary>
public class CustomerQueryResponse : BaseQueryResponse
{
    /// <summary>
    /// Customer name
    /// </summary>
    /// <example>Rafael</example>
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// Email
    /// </summary>
    /// <example>rafael.souza@gmail.com</example>
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    /// <summary>
    /// Address
    /// </summary>
    /// <example>Rua N, 723, Jardim Primavera - Ilheus/BA</example>
    [JsonPropertyName("address")]
    public string Address { get; set; }
}
