using AutoMapper;
using BillingManager.Domain.Entities;
using BillingManager.Domain.HttpClient;
using BillingManager.Infra.Data.Repositories.Interfaces;
using BillingManager.Services.HttpClients;
using MediatR;

namespace BillingManager.Application.Commands.Billing.ImportBilling;

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
            var importBillingResponse = new ImportBillingCommandResponse
            {
                InvoiceNumber = billing.InvoiceNumber
            };
            
            var customer = await customerRepository.GetByIdAsync(billing.Customer.Id);

            if (customer is null)
            {
                importBillingResponse.Errors.Add($"Customer {billing.Customer.Id} not found");
            }

            foreach (var productId in billing.BillingLines.Select(l => l.ProductId))
            {
                var product = await productRepository.GetByIdAsync(productId);

                if (product is null)
                {
                    importBillingResponse.Errors.Add($"Product {productId} not found");
                }
            }

            if (importBillingResponse.Errors.Any())
            {
                importBillingResponse.WasRegistered = false;
            }
            else
            {
                importBillingResponse.WasRegistered = true;
                var entity = mapper.Map<Domain.Entities.Billing>(billing);
                await billingRepository.CreateAsync(entity);
            }
            
            importBillingResponseList.Add(importBillingResponse);
        }
        
        return importBillingResponseList;
    }

    private static IList<BillingApiResponse> ClearInvalidAndDuplicatedBilling(IList<BillingApiResponse> billings)
        => billings.Where(b => !string.IsNullOrWhiteSpace(b.InvoiceNumber))
            .GroupBy(i => i.InvoiceNumber)
            .Select(g => g.First())
            .ToList();
}


