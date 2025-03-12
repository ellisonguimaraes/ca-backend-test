using BillingManager.Domain.HttpClient;

namespace BillingManager.Services.HttpClients;

/// <summary>
/// Http client to Billing API
/// </summary>
public interface IBillingHttpClient
{
    /// <summary>
    /// Get all billings in API
    /// </summary>
    /// <returns>Billing list</returns>
    Task<IList<BillingApiResponse>> GetAllBillingsAsync();   
}