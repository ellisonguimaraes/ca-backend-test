using BillingManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingManager.Infra.Data.Configurations;

/// <summary>
/// Billing lines entity database configuration
/// </summary>
public class BillingLineEntityConfiguration : BaseEntityConfiguration<BillingLine>
{
    #region Constants
    private const string TABLE_NAME = "billing_lines";
    private const string QUANTITY_DB_PROPERTY_NAME = "quantity";
    private const string UNIT_PRICE_DB_PROPERTY_NAME = "unit_price";
    private const string SUB_TOTAL_DB_PROPERTY_NAME = "sub_total";
    private const string PRODUCT_ID_DB_PROPERTY_NAME = "product_id";
    private const byte PRODUCT_ID_DB_PROPERTY_LENGTH = 36;
    private const string BILLING_ID_DB_PROPERTY_NAME = "billing_id";
    private const byte BILLING_ID_DB_PROPERTY_LENGTH = 36;
    #endregion

    public override void Configure(EntityTypeBuilder<BillingLine> builder)
    {
        builder.ToTable(TABLE_NAME);
        
        builder.Property(bl => bl.Quantity)
            .HasColumnName(QUANTITY_DB_PROPERTY_NAME)
            .IsRequired();

        builder.Property(bl => bl.UnitPrice)
            .HasColumnName(UNIT_PRICE_DB_PROPERTY_NAME)
            .IsRequired();

        builder.Property(bl => bl.SubTotal)
            .HasColumnName(SUB_TOTAL_DB_PROPERTY_NAME)
            .IsRequired();

        builder.Property(bl => bl.ProductId)
            .HasColumnName(PRODUCT_ID_DB_PROPERTY_NAME)
            .HasMaxLength(PRODUCT_ID_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.Property(bl => bl.BillingId)
            .HasColumnName(BILLING_ID_DB_PROPERTY_NAME)
            .HasMaxLength(BILLING_ID_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.HasOne(bl => bl.Product)
            .WithMany(p => p.BillingLines)
            .HasForeignKey(bl => bl.ProductId);

        builder.HasOne(bl => bl.Billing)
            .WithMany(b => b.BillingLines)
            .HasForeignKey(bl => bl.BillingId);

        base.Configure(builder);
    }
}