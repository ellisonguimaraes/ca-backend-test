using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BillingManager.Infra.CrossCutting.IoC;

/// <summary>
/// Logging builder extensions: configure serilog by configuration file
/// </summary>
public static class LoggingBuilderExtensions
{
    #region Constants
    private const string APPLICATION_PROPERTY = "Application";
    private const string APPLICATION_CONFIG_NAME = "ApplicationName";
    #endregion
    
    public static void LoggerConfiguration(this ILoggingBuilder builder, IConfiguration configuration)
    {
        builder.ClearProviders();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty(APPLICATION_PROPERTY, configuration[APPLICATION_CONFIG_NAME])
            .CreateLogger();

        builder.AddSerilog(logger);
    }
}