using System.Net.Mime;
using System.Text.Json;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.Resources;
using BillingManager.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.Application.ExceptionHandlers;

/// <summary>
/// Unsupported API version exception handler
/// </summary>
public class CustomUnsupportedApiVersionExceptionHandler(ILogger<CustomUnsupportedApiVersionExceptionHandler> logger) : IExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        var apiVersionException = (exception as CustomUnsupportedApiVersionException)!;
        
        var error = new ErrorResponse(ErrorsResource.UNSUPPORTED_API_VERSION_ERROR_CODE, ErrorsResource.UNSUPPORTED_API_VERSION_ERROR_MESSAGE);
                
        var response = new HttpResponse
        {
            Errors = [ error ]
        };
                
        logger.LogWarning(apiVersionException, 
            "[{ErrorCode}] {ErrorMessage}, Path: {Method} {Path}, TraceId: {TraceId}", 
            error.Code, 
            error.Message,
            context.Request.Method.ToUpper(), 
            context.Request.Path,
            response.TraceId);
                
        context.Response.Clear();
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}