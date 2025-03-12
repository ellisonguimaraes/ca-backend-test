using System.Text.Json.Serialization;

namespace BillingManager.Domain.HttpClient;

public sealed class BillingApiResponse
{
    [JsonPropertyName("invoice_number")]
    public string InvoiceNumber { get; set; }
    
    [JsonPropertyName("customer")]
    public CustomerApiResponse Customer { get; set; }
    
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("due_date")]
    public DateTime DueDate { get; set; }
    
    [JsonPropertyName("total_amount")]
    public decimal TotalAmount { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    
    [JsonPropertyName("lines")]
    public IList<BillingLineApiResponse> BillingLines { get; set; }
}

public sealed class CustomerApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("address")]
    public string Address { get; set; }
}

public sealed class BillingLineApiResponse
{
    [JsonPropertyName("productId")]
    public Guid ProductId { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    
    [JsonPropertyName("unit_price")]
    public decimal UnitPrice { get; set; }
    
    [JsonPropertyName("subtotal")]
    public decimal Subtotal { get; set; }
}