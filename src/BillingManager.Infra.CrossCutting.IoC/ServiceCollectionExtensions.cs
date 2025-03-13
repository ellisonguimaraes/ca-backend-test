using System.Diagnostics;
using BillingManager.Application.Behaviors;
using BillingManager.Application.Commands.Billing.ImportBilling;
using BillingManager.Application.Commands.Customers.Create;
using BillingManager.Application.Commands.Customers.Update;
using BillingManager.Application.Commands.Products.Create;
using BillingManager.Application.Commands.Products.Update;
using BillingManager.Application.ExceptionHandlers;
using BillingManager.Application.Notifications.DeleteAllPaginatedEntityInCache;
using BillingManager.Application.Notifications.UpdateEntityInCache;
using BillingManager.Application.Notifications.UpdatePaginateEntityInCache;
using BillingManager.Application.Profiles;
using BillingManager.Application.Queries.Customers.GetAll;
using BillingManager.Application.Queries.Customers.GetById;
using BillingManager.Application.Queries.Products.GetAll;
using BillingManager.Application.Queries.Products.GetById;
using BillingManager.Application.Validators;
using BillingManager.Domain.Configurations;
using BillingManager.Domain.Configurations.PerfomanceConfiguration;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.Utils;
using BillingManager.Infra.CrossCutting.IoC.Versioning;
using BillingManager.Infra.Data;
using BillingManager.Infra.Data.Repositories;
using BillingManager.Infra.Data.Repositories.Interfaces;
using BillingManager.Services;
using BillingManager.Services.HttpClients;
using BillingManager.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace BillingManager.Infra.CrossCutting.IoC;

public static class ServiceCollectionExtensions
{
    #region Constants
    const string CONNECTION_STRING_NAME = "DefaultConnection";
    private const bool ASSUME_DEFAULT_VERSION_WHEN_UNSPECIFIED = true;
    private const string API_DEFAULT_VERSION_PROPERTY = "ApiDefaultVersion";
    private const bool REPORT_API_VERSIONS = true;
    private const string HEADER_API_VERSION = "X-Version";
    private const string QUERY_STRING_API_VERSION = "api-version";
    private const string MEDIA_TYPE_API_VERSION = "ver";
    private const string FORMAT_API_VERSION = "'v'VVV";
    private const bool SUBSTITUTE_API_VERSION_IN_URL = true;
    private const string DOT = ".";
    #endregion
    
    /// <summary>
    /// Register command and queries handlers, notifications and behaviors
    /// </summary>
    public static IServiceCollection RegisterHandlersAndBehaviors(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateCustomerCommandHandler).Assembly);
        services.AddMediatR(typeof(UpdateCustomerCommandHandler).Assembly);
        services.AddMediatR(typeof(CreateProductCommandHandler).Assembly);
        services.AddMediatR(typeof(UpdateProductCommandHandler).Assembly);
        services.AddMediatR(typeof(GetAllCustomerQueryHandler).Assembly);
        services.AddMediatR(typeof(GetCustomerByIdQueryHandler).Assembly);
        services.AddMediatR(typeof(GetAllProductQueryHandler).Assembly);
        services.AddMediatR(typeof(GetProductByIdQueryHandler).Assembly);
        services.AddMediatR(typeof(ImportBillingCommandHandler).Assembly);

        services.AddTransient<INotificationHandler<UpdateEntityInCacheNotification<Customer>>, UpdateEntityInCacheNotificationHandler<Customer>>();
        services.AddTransient<INotificationHandler<UpdateEntityInCacheNotification<Product>>, UpdateEntityInCacheNotificationHandler<Product>>();
        
        services.AddTransient<INotificationHandler<UpdatePaginateEntityInCacheNotification<PagedList<Customer>, Customer>>, UpdatePaginateEntityInCacheNotificationHandler<Customer>>();
        services.AddTransient<INotificationHandler<UpdatePaginateEntityInCacheNotification<PagedList<Product>, Product>>, UpdatePaginateEntityInCacheNotificationHandler<Product>>();
        
        services.AddTransient<INotificationHandler<DeleteAllPaginatedEntityInCacheNotification<Customer>>, DeleteAllPaginatedEntityInCacheNotificationHandler<Customer>>();
        services.AddTransient<INotificationHandler<DeleteAllPaginatedEntityInCacheNotification<Product>>, DeleteAllPaginatedEntityInCacheNotificationHandler<Product>>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return services;
    }
    
    /// <summary>
    /// Register validators
    /// </summary>
    public static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();
        services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddScoped<IValidator<UpdateCustomerCommand>, UpdateCustomerCommandValidator>();
        services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
        services.AddScoped<IValidator<GetAllCustomerQuery>, GetAllCustomerQueryValidator>();
        services.AddScoped<IValidator<GetAllProductQuery>, GetAllProductQueryValidator>();
        
        return services;
    }
    
    /// <summary>
    /// Register mapper objects
    /// </summary>
    public static IServiceCollection RegisterMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerProfile));
        services.AddAutoMapper(typeof(ProductProfile));
        services.AddAutoMapper(typeof(BillingProfile));
        services.AddAutoMapper(typeof(BillingLineProfile));
        
        return services;
    }
    
    /// <summary>
    /// Register Http Clients
    /// </summary>
    public static IServiceCollection RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var billingApiSettings = configuration.GetSection(nameof(BillingApiSettings)).Get<BillingApiSettings>()!;
        
        services.AddHttpClient<IBillingHttpClient, BillingHttpClient>().ConfigureHttpClient(client => 
        {
            client.BaseAddress = new Uri(billingApiSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(billingApiSettings.TimeoutInSeconds);
        }).AddPolicyHandler((provider, _) =>
            {
                var logger = provider.GetRequiredService<ILogger<BillingHttpClient>>();
                
                return Policy
                    .Handle<HttpRequestException>()
                    .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .WaitAndRetryAsync(billingApiSettings.Retry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(billingApiSettings.RetryAttemptPowBase, retryAttempt)), 
                        (response, timeSpan, retryCount, context) =>
                        {
                            logger.LogWarning("Error in retry number {RetryCount} on {Endpoint}. Returned status code {StatusCode}. Next retry in {NextRetrySeconds} seconds. TraceId: {TraceId}. {@HttpResponseMessages}", 
                                retryCount, 
                                $"[{response.Result.RequestMessage?.Method.ToString().ToUpper()}] {response.Result.RequestMessage?.RequestUri}", 
                                (int)response.Result.StatusCode,
                                timeSpan.TotalSeconds,
                                Activity.Current?.Id,
                                response.Result);
                        });
            });

        services.AddSingleton(billingApiSettings);
        
        return services;
    }
    
    /// <summary>
    /// Configure and register db context, and register repositories
    /// </summary>
    public static IServiceCollection RegisterDbContextAndRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(CONNECTION_STRING_NAME));
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
    
    /// <summary>
    /// Register configuration files in container
    /// </summary>
    public static IServiceCollection RegisterConfigurationFiles(this IServiceCollection services, IConfiguration configuration)
    {
        var performanceConfiguration = configuration.GetSection(nameof(PerformanceConfiguration)).Get<PerformanceConfiguration>()!;
        services.AddSingleton(performanceConfiguration);
        
        return services;
    }
    
    /// <summary>
    /// Register exception handlers
    /// </summary>
    public static IServiceCollection RegisterExceptionHandlers(this IServiceCollection services)
    {
        services.AddSingleton<BusinessExceptionHandler>();
        services.AddSingleton<ApiExceptionHandler>();
        services.AddSingleton<CustomUnsupportedApiVersionExceptionHandler>();
        services.AddSingleton<ValidationExceptionHandler>();
        
        services.AddSingleton<IDictionary<Type, IExceptionHandler>>(provider => 
            new Dictionary<Type, IExceptionHandler>
            {
                { typeof(BusinessException), provider.GetRequiredService<BusinessExceptionHandler>() },
                { typeof(ApiException), provider.GetRequiredService<ApiExceptionHandler>() },
                { typeof(CustomUnsupportedApiVersionException), provider.GetRequiredService<CustomUnsupportedApiVersionExceptionHandler>() },
                { typeof(ValidationException), provider.GetRequiredService<ValidationExceptionHandler>() }
            });
        
        return services;
    }
    
    /// <summary>
    /// Register distributed cache services (redis)
    /// </summary>
    public static IServiceCollection RegisterDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>()!;
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings.Host;
            options.InstanceName = redisSettings.InstanceName;
        });

        services.AddScoped<ICachingService, RedisCachingService>();

        services.AddSingleton(redisSettings);
        
        return services;
    }
    
    /// <summary>
    /// Register versioning services
    /// </summary>
    public static void AddApiVersioningConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultVersion = configuration[API_DEFAULT_VERSION_PROPERTY]!.Split(DOT);
        var majorVersion = int.Parse(defaultVersion.First());
        var minorVersion = int.Parse(defaultVersion.Last());

        services.AddApiVersioning(options => {
            options.AssumeDefaultVersionWhenUnspecified = ASSUME_DEFAULT_VERSION_WHEN_UNSPECIFIED;
            options.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
            options.ReportApiVersions = REPORT_API_VERSIONS;
            options.ApiVersionReader = new MediaTypeApiVersionReader(MEDIA_TYPE_API_VERSION);
            options.ErrorResponses = new CustomVersioningErrorResponseProvider();
        });
        
        services.AddVersionedApiExplorer(setup => {
            setup.GroupNameFormat = FORMAT_API_VERSION;
            setup.SubstituteApiVersionInUrl = SUBSTITUTE_API_VERSION_IN_URL;
        });
    }
}