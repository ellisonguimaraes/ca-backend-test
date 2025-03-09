using System.Diagnostics;
using System.Text.Json.Serialization;

namespace BillingManager.Domain.Utils;

public class HttpResponse<T> where T : class
{
    [JsonPropertyName("trace_id")]
    public string? TraceId { get; set; } = Activity.Current?.Id;

    [JsonIgnore]
    public int StatusCode { get; set; }

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }

    [JsonPropertyName("errors")]
    public IEnumerable<ErrorResponse> Errors { get; set; } = [];
}

public sealed class HttpResponse :  HttpResponse<object>
{
}