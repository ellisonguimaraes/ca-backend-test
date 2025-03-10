using System.Text.Json.Serialization;

namespace BillingManager.Application.Commands;

/// <summary>
/// Base entity command response
/// </summary>
public abstract class BaseCommandResponse 
{
    /// <summary>
    /// Identifier
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Created date
    /// </summary>
    /// <example>2025-03-10T15:19:12.885Z</example>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Updated date
    /// </summary>
    /// <example>2025-03-10T15:19:12.885Z</example>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}