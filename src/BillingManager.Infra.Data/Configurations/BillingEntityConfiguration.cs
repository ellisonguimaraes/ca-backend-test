using BillingManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingManager.Infra.Data.Configurations;

/// <summary>
/// Billing entity database configuration
/// </summary>
public class BillingEntityConfiguration : BaseEntityConfiguration<Billing>
{
    #region Constants
    private const string TABLE_NAME = "billing";
    private const string INVOICE_NUMBER_DB_PROPERTY_NAME = "invoice_number";
    private const byte INVOICE_NUMBER_DB_PROPERTY_LENGTH = 36;
    private const string DATE_DB_PROPERTY_NAME = "date";
    private const string DUE_DATE_DB_PROPERTY_NAME = "due_date";
    private const string TOTAL_AMOUNT_DB_PROPERTY_NAME = "total_amount";
    private const string CURRENCY_DB_PROPERTY_NAME = "currency";
    private const byte CURRENCY_DB_PROPERTY_LENGTH = 10;
    private const string CUSTOMER_ID_DB_PROPERTY_NAME = "customer_id";
    private const byte CUSTOMER_ID_DB_PROPERTY_LENGTH = 36;
    #endregion

    public override void Configure(EntityTypeBuilder<Billing> builder)
    {
        builder.ToTable(TABLE_NAME);
        
        builder.Property(b => b.InvoiceNumber)
            .HasColumnName(INVOICE_NUMBER_DB_PROPERTY_NAME)
            .HasMaxLength(INVOICE_NUMBER_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.Property(b => b.Date)
            .HasColumnName(DATE_DB_PROPERTY_NAME)
            .IsRequired();

        builder.Property(b => b.DueDate)
            .HasColumnName(DUE_DATE_DB_PROPERTY_NAME)
            .IsRequired();

        builder.Property(b => b.TotalAmount)
            .HasColumnName(TOTAL_AMOUNT_DB_PROPERTY_NAME)
            .IsRequired();

        builder.Property(b => b.Currency)
            .HasColumnName(CURRENCY_DB_PROPERTY_NAME)
            .HasMaxLength(CURRENCY_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.Property(b => b.CustomerId)
            .HasColumnName(CUSTOMER_ID_DB_PROPERTY_NAME)
            .HasMaxLength(CUSTOMER_ID_DB_PROPERTY_LENGTH)
            .IsRequired();

        builder.HasOne(b => b.Customer)
            .WithMany(c => c.Billings)
            .HasForeignKey(b => b.CustomerId);

        base.Configure(builder);
    }
}