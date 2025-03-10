using System.Text.Json.Serialization;

namespace BillingManager.Application.Queries.Products;

/// <summary>
/// Generic product query response
/// </summary>
public class ProductQueryResponse : BaseQueryResponse
{
    /// <summary>
    /// Product name
    /// </summary>
    /// <example>Notebook</example>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
