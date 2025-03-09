using System.Net.Mime;
using System.Text.Json;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.Application.ExceptionHandlers;

/// <summary>
/// Business exception handle
/// </summary>
public class BusinessExceptionHandler(ILogger<BusinessExceptionHandler> logger) : IExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        var businessException = (exception as BusinessException)!;
        
        var error = new ErrorResponse(businessException.ErrorCode, businessException.Message);
                
        var response = new HttpResponse
        {
            Errors = [ error ]
        };
                
        logger.LogError(exception, 
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