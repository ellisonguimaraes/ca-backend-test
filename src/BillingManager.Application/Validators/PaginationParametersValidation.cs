using BillingManager.Domain.Resources;
using BillingManager.Domain.Utils;
using FluentValidation;

namespace BillingManager.Application.Validators;

public abstract class PaginationParametersValidation : AbstractValidator<PaginationParameters>
{
    protected PaginationParametersValidation()
    {
        RuleFor(pp => pp.PageNumber)
            .NotEmpty()
                .WithMessage(ErrorsResource.VALIDATION_NOT_NULL_ERROR_MESSAGE)
                .WithErrorCode(ErrorsResource.VALIDATION_NOT_NULL_ERROR_CODE)
            .GreaterThanOrEqualTo(int.Parse(ErrorsResource.MIN_LIMIT_PAGE_NUMBER))
                .WithMessage(ErrorsResource.VALIDATION_GREATER_THEN_OR_EQUAL_ERROR_MESSAGE)
                .WithErrorCode(ErrorsResource.VALIDATION_GREATER_THEN_OR_EQUAL_ERROR_CODE);
        
        RuleFor(pp => pp.PageSize)
            .NotEmpty()
                .WithMessage(ErrorsResource.VALIDATION_NOT_NULL_ERROR_MESSAGE)
                .WithErrorCode(ErrorsResource.VALIDATION_NOT_NULL_ERROR_CODE)
            .GreaterThanOrEqualTo(int.Parse(ErrorsResource.MIN_LIMIT_PAGE_SIZE))
                .WithMessage(ErrorsResource.VALIDATION_GREATER_THEN_OR_EQUAL_ERROR_MESSAGE)
                .WithErrorCode(ErrorsResource.VALIDATION_GREATER_THEN_OR_EQUAL_ERROR_CODE)
            .LessThanOrEqualTo(int.Parse(ErrorsResource.LIMIT_PAGE_SIZE))
                .WithMessage(ErrorsResource.VALIDATION_LESS_THEN_OR_EQUAL_ERROR_MESSAGE)
                .WithErrorCode(ErrorsResource.VALIDATION_LESS_THEN_OR_EQUAL_ERROR_CODE);
    }
}