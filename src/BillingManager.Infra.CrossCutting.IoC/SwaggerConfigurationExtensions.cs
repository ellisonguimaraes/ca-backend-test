using BillingManager.Domain.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BillingManager.Infra.CrossCutting.IoC;

/// <summary>
/// Swagger configuration extensions
/// </summary>
public static class SwaggerConfigurationExtensions
{
    #region Constants
    private const string PROJECTS_APP_SETTINGS_PROPERTY_NAME  = "ProjectAssemblies";
    #endregion
    
    /// <summary>
    /// Register and configure Swagger Open API
    /// </summary>
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerInfoSettings = configuration.GetSection(nameof(SwaggerInfoSettings)).Get<SwaggerInfoSettings>()!;
        var assemblies = configuration.GetSection(PROJECTS_APP_SETTINGS_PROPERTY_NAME).Get<IList<string>>()!;
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(swaggerInfoSettings.Version, new OpenApiInfo
            {
                Title = swaggerInfoSettings.Title, 
                Version = swaggerInfoSettings.Version,
                Description = swaggerInfoSettings.Description,
                Contact = new OpenApiContact {
                    Name = swaggerInfoSettings.Contact.Name,
                    Email = swaggerInfoSettings.Contact.Email,
                    Url = new Uri(swaggerInfoSettings.Contact.Url)
                }
            });
    
            options.EnableAnnotations();
    
            var xmlPathBase = AppContext.BaseDirectory;

            foreach (var assembly in assemblies)
            {
                var xmlPath = Path.Combine(xmlPathBase, $"{assembly}.xml");
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            }
        });

        services.AddSingleton(swaggerInfoSettings);
        
        return services;
    }
    
    /// <summary>
    /// Configure Swagger in application builder
    /// </summary>
    public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
    {
        var swaggerInfoSettings = app.ApplicationServices.GetRequiredService<SwaggerInfoSettings>();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = swaggerInfoSettings.RoutePrefix;
            options.SwaggerEndpoint(swaggerInfoSettings.SwaggerEndpoint, swaggerInfoSettings.Version);
        });
        
        return app;
    }
}