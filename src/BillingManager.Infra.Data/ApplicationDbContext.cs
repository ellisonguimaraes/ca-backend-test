using BillingManager.Domain.Entities;
using BillingManager.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BillingManager.Infra.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    #region Constants
    private const string NPGSQL_ENABLELEGACYTIMESTAMPBEHAVIOR_NAME = "Npgsql.EnableLegacyTimestampBehavior";
    private const bool HAS_NPGSQL_ENABLELEGACYTIMESTAMPBEHAVIOR = true;
    #endregion

    public DbSet<Billing> Billings { get; set; }
    public DbSet<BillingLine> BillingLines { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new BillingEntityConfiguration().Configure(modelBuilder.Entity<Billing>());
        new BillingLineEntityConfiguration().Configure(modelBuilder.Entity<BillingLine>());
        new CustomerEntityTypeConfiguration().Configure(modelBuilder.Entity<Customer>());
        new ProductEntityConfiguration().Configure(modelBuilder.Entity<Product>());
        
        AppContext.SetSwitch(NPGSQL_ENABLELEGACYTIMESTAMPBEHAVIOR_NAME, HAS_NPGSQL_ENABLELEGACYTIMESTAMPBEHAVIOR);

        base.OnModelCreating(modelBuilder);
    }
}