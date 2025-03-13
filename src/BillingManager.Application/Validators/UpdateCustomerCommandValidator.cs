using BillingManager.Application.Commands.Customers.Update;
using BillingManager.Domain.Resources;
using FluentValidation;

namespace BillingManager.Application.Validators;

/// <summary>
/// UpdateCustomerCommand validator
/// </summary>
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_NULL_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_NULL_ERROR_CODE);
        
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_CODE);
        
        RuleFor(c => c.Address)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_CODE);
        
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_CODE)
            .EmailAddress().WithMessage(ErrorsResource.VALIDATION_IS_INVALID_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_IS_INVALID_ERROR_CODE);
    }
}