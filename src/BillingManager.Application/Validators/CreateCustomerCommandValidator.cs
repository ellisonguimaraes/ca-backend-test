using BillingManager.Application.Commands.Customers.Create;
using BillingManager.Domain.Resources;
using FluentValidation;

namespace BillingManager.Application.Validators;

/// <summary>
/// CreateCustomerCommand validator
/// </summary>
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_CODE);
        
        RuleFor(c => c.Address)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_CODE);
        
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_CODE)
            .EmailAddress().WithMessage(ErrorsResource.VALIDATION_IS_INVALID_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_IS_INVALID_ERROR_CODE);
    }
}