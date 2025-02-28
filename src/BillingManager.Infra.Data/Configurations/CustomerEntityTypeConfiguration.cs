using BillingManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingManager.Infra.Data.Configurations;

/// <summary>
/// Customer entity database configuration
/// </summary>
public class CustomerEntityTypeConfiguration : BaseEntityConfiguration<Customer>
{
    #region Constants
    private const string TABLE_NAME = "customer";
    private const string CUSTOMER_NAME_DB_PROPERTY_NAME = "name";
    private const byte CUSTOMER_NAME_DB_PROPERTY_LENGTH = 100;
    private const string EMAIL_DB_PROPERTY_NAME = "email";
    private const byte EMAIL_DB_PROPERTY_LENGTH = 100;
    private const string ADDRESS_DB_PROPERTY_NAME = "address";
    private const byte ADDRESS_DB_PROPERTY_LENGTH = 150;
    #endregion

    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(TABLE_NAME);
        
        builder.Property(c => c.Name)
            .HasColumnName(CUSTOMER_NAME_DB_PROPERTY_NAME)
            .HasMaxLength(CUSTOMER_NAME_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnName(EMAIL_DB_PROPERTY_NAME)
            .HasMaxLength(EMAIL_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.Property(c => c.Address)
            .HasColumnName(ADDRESS_DB_PROPERTY_NAME)
            .HasMaxLength(ADDRESS_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.HasMany(c => c.Billings)
            .WithOne(b => b.Customer)
            .HasForeignKey(b => b.CustomerId);
            
        base.Configure(builder);
    }
}