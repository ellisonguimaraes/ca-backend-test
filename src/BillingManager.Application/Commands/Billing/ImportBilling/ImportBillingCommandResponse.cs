using System.Text.Json.Serialization;

namespace BillingManager.Application.Commands.Billing.ImportBilling;

public sealed class ImportBillingCommandResponse
{
    [JsonPropertyName("was_registered")]
    public bool WasRegistered { get; set; }
    
    [JsonPropertyName("invoice_number")]
    public string InvoiceNumber { get; set; }
    
    [JsonPropertyName("errors")]
    public IList<string> Errors { get; set; } = [];
}