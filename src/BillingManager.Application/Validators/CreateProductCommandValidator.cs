using BillingManager.Application.Commands.Products.Create;
using BillingManager.Domain.Resources;
using FluentValidation;

namespace BillingManager.Application.Validators;

/// <summary>
/// CreateProductCommand validator
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_MESSAGE).WithErrorCode(ErrorsResource.VALIDATION_NOT_EMPTY_ERROR_CODE);
    }
}