using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Queries.Products.GetById;

/// <summary>
/// Get product by id query
/// </summary>
public sealed class GetProductByIdQuery : IRequest<ProductQueryResponse>
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}