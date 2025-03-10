using System.Reflection;
using System.Text.Json;
using BillingManager.API.Middlewares;
using BillingManager.Infra.CrossCutting.IoC;
using MediatR;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

builder.Services.AddControllers();

builder.Logging.LoggerConfiguration(builder.Configuration);

builder.Services
    .RegisterHandlersAndBehaviors()
    .RegisterMappers()
    .RegisterDbContextAndRepositories(builder.Configuration)
    .RegisterConfigurationFiles(builder.Configuration)
    .RegisterDistributedCache(builder.Configuration)
    .RegisterExceptionHandlers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();