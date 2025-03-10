using System.Diagnostics;
using System.Text.Json.Serialization;

namespace BillingManager.Domain.Utils;

/// <summary>
/// Generic http response
/// </summary>
public class HttpResponse<T> where T : class
{
    /// <summary>
    /// Trace identifier
    /// </summary>
    /// <example>00-4e1e307d7eb6acc951186c77902f3cc6-87506b389e488a5c-00</example>
    [JsonPropertyName("trace_id")]
    public string? TraceId { get; set; } = Activity.Current?.Id;

    [JsonIgnore]
    public int StatusCode { get; set; }

    /// <summary>
    /// Response data
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T Data { get; set; } = null!;
    
    /// <summary>
    /// Error code and error message
    /// </summary>
    [JsonPropertyName("errors")]
    public IEnumerable<ErrorResponse> Errors { get; set; } = [];
}

/// <summary>
/// Generic http response
/// </summary>
public sealed class HttpResponse :  HttpResponse<object>
{
}