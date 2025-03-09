using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Commands.Products.Update;

/// <summary>
/// Update product command
/// </summary>
public sealed class UpdateProductCommand : IRequest<ProductCommandResponse>
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}