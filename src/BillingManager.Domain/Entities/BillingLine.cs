namespace BillingManager.Domain.Entities;

public class BillingLine : BaseEntity
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }

    // Relationship
    public virtual Product Product { get; set; } = null!;
    public Guid ProductId { get; set; }

    public virtual Billing Billing { get; set; } = null!;
    public Guid BillingId { get; set; }
}
