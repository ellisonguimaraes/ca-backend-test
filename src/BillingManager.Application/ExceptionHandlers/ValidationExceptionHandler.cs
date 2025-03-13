using System.Net.Mime;
using System.Text.Json;
using BillingManager.Domain.Resources;
using BillingManager.Domain.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.Application.ExceptionHandlers;

/// <summary>
/// Validation error exception handler
/// </summary>
public class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        var validationException = (exception as ValidationException)!;
        
        var response = new HttpResponse
        {
            Errors = validationException.Errors.Select(error => new ErrorResponse(error.ErrorCode, error.ErrorMessage)).ToList()
        };
                
        logger.LogError(validationException, 
            "{ErrorMessage}, Path: {Method} {Path}, TraceId: {TraceId}", 
            ErrorsResource.VALIDATION_ERROR_MESSAGE,
            context.Request.Method.ToUpper(), 
            context.Request.Path,
            response.TraceId);
                
        context.Response.Clear();
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}