using System.Text.Json.Serialization;
using MediatR;

namespace BillingManager.Application.Commands.Customers.Create;

/// <summary>
/// Create customer command
/// </summary>
public sealed class CreateCustomerCommand : IRequest<CustomerCommandResponse>
{
    /// <summary>
    /// Customer identifier
    /// </summary>
    /// <example>582ca72e-cd4c-4071-bbcc-29e6bae4eff6</example>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
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
