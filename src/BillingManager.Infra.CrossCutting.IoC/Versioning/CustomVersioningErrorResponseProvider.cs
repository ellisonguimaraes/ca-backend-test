using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using BillingManager.Domain.Exceptions;

namespace BillingManager.Infra.CrossCutting.IoC.Versioning;

/// <summary>
/// Custom versioning treatment when version not exists
/// </summary>
public class CustomVersioningErrorResponseProvider : IErrorResponseProvider
{
    public IActionResult CreateResponse(ErrorResponseContext context)
    {
        throw new CustomUnsupportedApiVersionException(context);
    }
}