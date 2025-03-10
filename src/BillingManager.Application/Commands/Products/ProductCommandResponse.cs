using System.Text.Json.Serialization;

namespace BillingManager.Application.Commands.Products;

/// <summary>
/// Generic product command response
/// </summary>
public class ProductCommandResponse : BaseCommandResponse
{
    /// <summary>
    /// Product name
    /// </summary>
    /// <example>Notebook</example>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}