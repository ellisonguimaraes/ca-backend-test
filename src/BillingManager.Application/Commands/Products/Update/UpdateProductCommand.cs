using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Commands.Products.Update;

/// <summary>
/// Update product command
/// </summary>
public sealed class UpdateProductCommand : IRequest<ProductCommandResponse>
{
    [JsonPropertyName("id")]
    [JsonIgnore]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Product name
    /// </summary>
    /// <example>Notebook</example>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}