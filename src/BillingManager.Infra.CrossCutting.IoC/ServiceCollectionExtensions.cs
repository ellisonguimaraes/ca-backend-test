using BillingManager.Application.Behaviors;
using BillingManager.Application.Commands.Customers.Create;
using BillingManager.Application.Commands.Customers.Update;
using BillingManager.Application.Commands.Products.Create;
using BillingManager.Application.Commands.Products.Update;
using BillingManager.Application.ExceptionHandlers;
using BillingManager.Application.Profiles;
using BillingManager.Application.Queries.Customers.GetAll;
using BillingManager.Application.Queries.Customers.GetById;
using BillingManager.Application.Queries.Products.GetAll;
using BillingManager.Application.Queries.Products.GetById;
using BillingManager.Domain.Configurations.PerfomanceConfiguration;
using BillingManager.Domain.Exceptions;
using BillingManager.Infra.Data;
using BillingManager.Infra.Data.Repositories;
using BillingManager.Infra.Data.Repositories.Interfaces;
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

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        
        return services;
    }

    public static IServiceCollection RegisterMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerProfile));
        services.AddAutoMapper(typeof(ProductProfile));
        
        return services;
    }

    public static IServiceCollection RegisterDbContextAndRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(CONNECTION_STRING_NAME));
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
    
    public static IServiceCollection RegisterConfigurationFiles(this IServiceCollection services, IConfiguration configuration)
    {
        var performanceConfiguration = configuration.GetSection(nameof(PerformanceConfiguration)).Get<PerformanceConfiguration>()!;
        services.AddSingleton(performanceConfiguration);
        
        return services;
    }

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
}