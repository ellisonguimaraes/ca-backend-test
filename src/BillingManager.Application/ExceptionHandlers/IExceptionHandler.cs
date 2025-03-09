using Microsoft.AspNetCore.Http;

namespace BillingManager.Application.ExceptionHandlers;

/// <summary>
/// Exception handle interface
/// </summary>
public interface IExceptionHandler
{
    Task HandleAsync(HttpContext context, Exception exception);
}