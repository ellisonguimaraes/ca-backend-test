using System.Net.Mime;
using System.Text.Json;
using BillingManager.Application.ExceptionHandlers;
using BillingManager.Domain.Resources;
using BillingManager.Domain.Utils;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.API.Middlewares;

/// <summary>
/// Middleware that handles all request exceptions
/// </summary>
public sealed class ExceptionMiddleware(
    RequestDelegate next,
    IDictionary<Type, IExceptionHandler> handlers,
    ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (handlers.TryGetValue(ex.GetType(), out var handler))
            {
                await handler.HandleAsync(context, ex);
            }
            else
            {
                var error = new ErrorResponse(ErrorsResource.NOT_MAPPED_EXCEPTION_ERROR_CODE, ErrorsResource.NOT_MAPPED_EXCEPTION_ERROR_MESSAGE);
                
                var response = new HttpResponse
                {
                    Errors = [ error ]
                };
                
                logger.LogError(ex, 
                    "[{ErrorCode}] {ErrorMessage}, Path: {Method} {Path}, TraceId: {TraceId}", 
                    error.Code, 
                    error.Message,
                    context.Request.Method.ToUpper(), 
                    context.Request.Path,
                    response.TraceId);
                
                context.Response.Clear();
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}