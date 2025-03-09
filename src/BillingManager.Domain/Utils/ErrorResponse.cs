using System.Text.Json.Serialization;

namespace BillingManager.Domain.Utils;

public sealed class ErrorResponse(string code, string message)
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = code;

    [JsonPropertyName("message")]
    public string Message { get; set; } = message;
}
