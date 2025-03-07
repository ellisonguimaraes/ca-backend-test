using BillingManager.Application.Commands.Customers.Create;
using BillingManager.Application.Commands.Customers.Update;
using BillingManager.Application.Profiles;
using BillingManager.Application.Queries.Customers.GetAll;
using BillingManager.Application.Queries.Customers.GetById;
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

    public static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CreateCustomerCommandHandler).Assembly);
        services.AddMediatR(typeof(GetAllCustomerQueryHandler).Assembly);
        services.AddMediatR(typeof(GetCustomerByIdQueryHandler).Assembly);
        services.AddMediatR(typeof(UpdateCustomerCommandHandler).Assembly);
        
        return services;
    }

    public static IServiceCollection RegisterMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerProfile));
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
}