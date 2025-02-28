namespace BillingManager.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    // Relationship
    public virtual IList<Billing> Billings { get; set; } = null!;
}
