using BillingManager.Infra.Data;
using BillingManager.Infra.Data.Repositories;
using BillingManager.Infra.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

#region Constants
const string CONNECTION_STRING_NAME = "DefaultConnection";
#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Entity Framework Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(CONNECTION_STRING_NAME));
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();