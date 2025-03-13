using Microsoft.AspNetCore.Mvc.Versioning;

namespace BillingManager.Domain.Exceptions;

public class CustomUnsupportedApiVersionException(ErrorResponseContext context) : Exception
{
    public ErrorResponseContext Context { get; } = context;
}