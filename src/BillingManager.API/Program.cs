using BillingManager.API.Middlewares;
using BillingManager.Infra.CrossCutting.IoC;

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
    .RegisterExceptionHandlers();

var app = builder.Build();

app.ConfigureSwagger();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();