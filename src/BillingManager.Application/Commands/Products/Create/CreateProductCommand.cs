using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Commands.Products.Create;

/// <summary>
/// Create product command
/// </summary>
public sealed class CreateProductCommand : IRequest<ProductCommandResponse>
{
    /// <summary>
    /// Product identifier
    /// </summary>
    /// <example>582ca72e-cd4c-4071-bbcc-29e6bae4eff6</example>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Product name
    /// </summary>
    /// <example>Notebook</example>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
