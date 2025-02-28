namespace BillingManager.Domain.Entities;

public class Billing : BaseEntity
{
    public string InvoiceNumber { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; }

    // Relationship
    public Customer Customer { get; set; } = null!;
    public Guid CustomerId { get; set; }
    
    public virtual IList<BillingLine> BillingLines { get; set; } = null!;
}