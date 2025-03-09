namespace BillingManager.Domain.Configurations.PerfomanceConfiguration;

public sealed class PerformanceConfiguration
{
    public IList<Handler> Handlers { get; set; }
    public long DefaultTimeoutInMilliseconds { get; set; }
}