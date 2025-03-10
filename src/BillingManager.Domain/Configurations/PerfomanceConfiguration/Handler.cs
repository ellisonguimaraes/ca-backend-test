namespace BillingManager.Domain.Configurations.PerfomanceConfiguration;

/// <summary>
/// Only handle performance information (appsettings)
/// </summary>
public sealed class Handler
{
    public string HandlerName { get; set; }
    public long TimeoutInMilliseconds { get; set; }
}