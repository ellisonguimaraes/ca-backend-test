namespace BillingManager.Domain.Configurations;

/// <summary>
/// Billing api configuration (appsettings)
/// </summary>
public sealed class BillingApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string BillingPath { get; set; } = string.Empty;
    public int TimeoutInSeconds { get; set; }
    public int Retry { get; set; }
    public int RetryAttemptPowBase { get; set; }
}