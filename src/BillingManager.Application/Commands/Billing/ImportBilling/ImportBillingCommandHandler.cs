using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Domain.HttpClient;
using BillingManager.Infra.Data.Repositories.Interfaces;
using BillingManager.Services.HttpClients;
using MediatR;

namespace BillingManager.Application.Commands.Billing.ImportBilling;

/// <summary>
/// Command to import billing from external request
/// </summary>
public class ImportBillingCommandHandler(
    IBillingHttpClient billingHttpClient,
    IRepository<Customer> customerRepository,
    IRepository<Product> productRepository,
    IRepository<Domain.Entities.Billing> billingRepository,
    IMapper mapper) : IRequestHandler<ImportBillingCommand, IList<ImportBillingCommandResponse>>
{
    public async Task<IList<ImportBillingCommandResponse>> Handle(ImportBillingCommand request, CancellationToken cancellationToken)
    {
        var importBillingResponseList =  new List<ImportBillingCommandResponse>();
        
        var billings = await billingHttpClient.GetAllBillingsAsync();
        
        billings = ClearInvalidAndDuplicatedBilling(billings);
        
        foreach (var billing in billings)
        {
            var importBillingResponse = new ImportBillingCommandResponse();

            await CheckExistsCustomer(billing.Customer.Id, importBillingResponse);
            await CheckExistsProducts(billing.BillingLines.Select(bl => bl.ProductId).ToList(), importBillingResponse);
            
            if (!importBillingResponse.Errors.Any())
            {
                importBillingResponse.WasRegistered = true;
                var entity = mapper.Map<Domain.Entities.Billing>(billing);
                await billingRepository.CreateAsync(entity);
            }
            else
            {
                importBillingResponse.WasRegistered = false;
            }
            
            importBillingResponse.InvoiceNumber = billing.InvoiceNumber;
            importBillingResponseList.Add(importBillingResponse);
        }
        
        return importBillingResponseList;
    }

    /// <summary>
    /// Check if customer exist
    /// </summary>
    /// <param name="customerId">Customer identifier</param>
    /// <param name="importBillingCommandResponse">Response model</param>
    private async Task CheckExistsCustomer(Guid customerId, ImportBillingCommandResponse importBillingCommandResponse)
    {
        if (await customerRepository.GetByIdAsync(customerId) is null)
            importBillingCommandResponse.Errors.Add($"Customer {customerId} not found");
    }
    
    /// <summary>
    /// Check if products exists
    /// </summary>
    /// <param name="products">Product identifiers</param>
    /// <param name="importBillingCommandResponse">Response model</param>
    private async Task CheckExistsProducts(IList<Guid> products, ImportBillingCommandResponse importBillingCommandResponse)
    {
        foreach (var product in products)
            if (await productRepository.GetByIdAsync(product) is null)
                importBillingCommandResponse.Errors.Add($"Product {product} not found");
    }
    
    /// <summary>
    /// Clear invalid billing in API
    /// </summary>
    /// <param name="billings">Billing API response</param>
    /// <returns>Cleaned billings</returns>
    private static IList<BillingApiResponse> ClearInvalidAndDuplicatedBilling(IList<BillingApiResponse> billings)
        => billings.Where(b => !string.IsNullOrWhiteSpace(b.InvoiceNumber))
            .GroupBy(i => i.InvoiceNumber)
            .Select(g => g.First())
            .ToList();
}


