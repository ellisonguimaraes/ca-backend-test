using System.Text.Json;
using BillingManager.Domain.Configurations;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.HttpClient;

namespace BillingManager.Services.HttpClients;

public sealed class BillingHttpClient(HttpClient client, BillingApiSettings settings) : IBillingHttpClient
{
    public async Task<IList<BillingApiResponse>> GetAllBillingsAsync()
    {
        var response = await client.GetAsync(settings.BillingPath);

        if (!response.IsSuccessStatusCode)
            throw new ApiException(response);
        
        var body = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IList<BillingApiResponse>>(body)!;
    }
}