using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Commands.Products.Create;

/// <summary>
/// Create product command
/// </summary>
public sealed class CreateProductCommand : IRequest<ProductCommandResponse>
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
