namespace BillingManager.Domain.Configurations.PerfomanceConfiguration;

/// <summary>
/// Performance handlers infos (appsettings)
/// </summary>
public sealed class PerformanceConfiguration
{
    public IList<Handler> Handlers { get; set; }
    public long DefaultTimeoutInMilliseconds { get; set; }
}