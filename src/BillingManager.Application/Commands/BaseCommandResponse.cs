using System.Text.Json.Serialization;

namespace BillingManager.Application.Commands;

/// <summary>
/// Base entity command response
/// </summary>
public abstract class BaseCommandResponse 
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}