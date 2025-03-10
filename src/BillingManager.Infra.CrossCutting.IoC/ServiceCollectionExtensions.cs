using BillingManager.Application.Behaviors;
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
using BillingManager.Domain.Configurations;
using BillingManager.Domain.Configurations.PerfomanceConfiguration;
using BillingManager.Domain.Entities;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.Utils;
using BillingManager.Infra.Data;
using BillingManager.Infra.Data.Repositories;
using BillingManager.Infra.Data.Repositories.Interfaces;
using BillingManager.Services;
using BillingManager.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BillingManager.Infra.CrossCutting.IoC;

public static class ServiceCollectionExtensions
{
    #region Constants
    const string CONNECTION_STRING_NAME = "DefaultConnection";
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

        services.AddTransient<INotificationHandler<UpdateEntityInCacheNotification<Customer>>, UpdateEntityInCacheNotificationHandler<Customer>>();
        services.AddTransient<INotificationHandler<UpdateEntityInCacheNotification<Product>>, UpdateEntityInCacheNotificationHandler<Product>>();
        
        services.AddTransient<INotificationHandler<UpdatePaginateEntityInCacheNotification<PagedList<Customer>, Customer>>, UpdatePaginateEntityInCacheNotificationHandler<Customer>>();
        services.AddTransient<INotificationHandler<UpdatePaginateEntityInCacheNotification<PagedList<Product>, Product>>, UpdatePaginateEntityInCacheNotificationHandler<Product>>();
        
        services.AddTransient<INotificationHandler<DeleteAllPaginatedEntityInCacheNotification<Customer>>, DeleteAllPaginatedEntityInCacheNotificationHandler<Customer>>();
        services.AddTransient<INotificationHandler<DeleteAllPaginatedEntityInCacheNotification<Product>>, DeleteAllPaginatedEntityInCacheNotificationHandler<Product>>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        
        return services;
    }
    
    /// <summary>
    /// Register mapper objects
    /// </summary>
    public static IServiceCollection RegisterMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerProfile));
        services.AddAutoMapper(typeof(ProductProfile));
        
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

        services.AddSingleton<IDictionary<Type, IExceptionHandler>>(provider => 
            new Dictionary<Type, IExceptionHandler>
            {
                { typeof(BusinessException), provider.GetRequiredService<BusinessExceptionHandler>() }
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
}