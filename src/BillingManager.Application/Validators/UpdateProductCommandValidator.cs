using BillingManager.Application.Commands.Products.Update;
using BillingManager.Domain.Resources;
using FluentValidation;

namespace BillingManager.Application.Validators;

/// <summary>
/// UpdateProductCommand validator
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_NULL_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_NULL_ERROR_CODE);

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_CODE);
    }
}