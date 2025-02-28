using BillingManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingManager.Infra.Data.Configurations;

/// <summary>
/// Product entity database configuration
/// </summary>
public class ProductEntityConfiguration : BaseEntityConfiguration<Product>
{
    #region Constants
    private const string TABLE_NAME = "product";
    private const string PRODUCT_NAME_DB_PROPERTY_NAME = "name";
    private const byte PRODUCT_NAME_DB_PROPERTY_LENGTH = 100;
    #endregion

    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TABLE_NAME);
        
        builder.Property(p => p.Name)
            .HasColumnName(PRODUCT_NAME_DB_PROPERTY_NAME)
            .HasMaxLength(PRODUCT_NAME_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.HasMany(p => p.BillingLines)
            .WithOne(bl => bl.Product)
            .HasForeignKey(bl => bl.ProductId);

        base.Configure(builder);
    }
}