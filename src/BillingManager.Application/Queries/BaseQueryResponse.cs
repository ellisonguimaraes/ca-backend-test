using System.Text.Json.Serialization;

namespace BillingManager.Application.Queries;

/// <summary>
/// Base entity queries response
/// </summary>
public abstract class BaseQueryResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}