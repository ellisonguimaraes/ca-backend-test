using MediatR;

namespace BillingManager.Application.Commands.Billing.ImportBilling;

public sealed class ImportBillingCommand : IRequest<IList<ImportBillingCommandResponse>>
{
}