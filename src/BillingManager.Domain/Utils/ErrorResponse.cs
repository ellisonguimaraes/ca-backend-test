using System.Text.Json.Serialization;

namespace BillingManager.Domain.Utils;

/// <summary>
/// Response error
/// </summary>
public sealed class ErrorResponse(string code, string message)
{
    /// <summary>
    /// Error code
    /// </summary>
    /// <example>ERR001</example>
    [JsonPropertyName("code")]
    public string Code { get; set; } = code;
    
    /// <summary>
    /// Error message
    /// </summary>
    /// <example>Error occurred message</example>
    [JsonPropertyName("message")]
    public string Message { get; set; } = message;
}
