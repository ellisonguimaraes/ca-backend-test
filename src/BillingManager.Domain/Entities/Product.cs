namespace BillingManager.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }

    // Relationship
    public virtual IList<BillingLine> BillingLines { get; set; } = null!;
}