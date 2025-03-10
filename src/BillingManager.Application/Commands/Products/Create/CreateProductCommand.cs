using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Commands.Products.Create;

/// <summary>
/// Create product command
/// </summary>
public sealed class CreateProductCommand : IRequest<ProductCommandResponse>
{
    /// <summary>
    /// Product name
    /// </summary>
    /// <example>Notebook</example>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
