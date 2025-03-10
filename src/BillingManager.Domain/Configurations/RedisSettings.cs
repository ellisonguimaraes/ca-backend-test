namespace BillingManager.Domain.Configurations;

/// <summary>
/// Redis Settings (appsettings values)
/// </summary>
public sealed class RedisSettings
{
    public string Host { get; set; }
    public string InstanceName { get; set; }
}