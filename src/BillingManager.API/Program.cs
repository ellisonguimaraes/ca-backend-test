using BillingManager.API.Middlewares;
using BillingManager.Infra.CrossCutting.IoC;
using Microsoft.AspNetCore.Mvc;

#region Constants
const string PATH_HEALTH_CHECK = "/health";
#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Logging.LoggerConfiguration(builder.Configuration);

builder.Services
    .ConfigureSwagger(builder.Configuration)
    .RegisterHandlersAndBehaviors()
    .RegisterMappers()
    .RegisterDbContextAndRepositories(builder.Configuration)
    .RegisterConfigurationFiles(builder.Configuration)
    .RegisterDistributedCache(builder.Configuration)
    .RegisterExceptionHandlers()
    .RegisterHttpClients(builder.Configuration)
    .AddApiVersioningConfiguration(builder.Configuration);

builder.Services.AddHealthChecks();

// Clear .NET Built-in Validator (in the action inside the controller)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.ConfigureSwagger();

app.MapHealthChecks(PATH_HEALTH_CHECK);

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();