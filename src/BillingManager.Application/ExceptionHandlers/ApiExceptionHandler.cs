using System.Net.Mime;
using System.Text.Json;
using BillingManager.Domain.Exceptions;
using BillingManager.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.Application.ExceptionHandlers;

/// <summary>
/// Api Exception Handler
/// </summary>
public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        var apiException = (exception as ApiException)!;
        
        var error = new ErrorResponse(apiException.ErrorCode, apiException.Message);
                
        var response = new HttpResponse
        {
            Errors = [ error ]
        };
        
        logger.LogError(apiException, 
            "The external request to [{RequestMessageMethod}] {RequestMessageUri} failed with status code {ResponseStatusCode}. Path: {Method} {Path}, TraceId: {TraceId}, Body: {Body}", 
            apiException.HttpResponseMessage.RequestMessage?.Method,
            apiException.HttpResponseMessage.RequestMessage?.RequestUri,
            (int)apiException.HttpResponseMessage.StatusCode,
            context.Request.Method.ToUpper(), 
            context.Request.Path,
            response.TraceId,
            await apiException.HttpResponseMessage.Content.ReadAsStringAsync());
                
        context.Response.Clear();
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}