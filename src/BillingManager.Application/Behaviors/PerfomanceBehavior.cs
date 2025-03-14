using System.Diagnostics;
using BillingManager.Domain.Configurations.PerfomanceConfiguration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BillingManager.Application.Behaviors;

/// <summary>
/// Performance behavior: Calculates the execution time of the handler and logs it if the time exceeds the allowed limit.
/// </summary>
public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, PerformanceConfiguration configuration) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var timer = new Stopwatch();
        
        timer.Start();
        var response = await next();
        timer.Stop();

        var elapsedMilliseconds = timer.ElapsedMilliseconds;
        
        var handlerTimeout = configuration.Handlers.FirstOrDefault(h => h.HandlerName.Equals(typeof(TRequest).Name, StringComparison.OrdinalIgnoreCase));

        if (handlerTimeout is not null)
        {
            if (elapsedMilliseconds > handlerTimeout.TimeoutInMilliseconds)
                logger.LogWarning("{Handler} executed in {ElapsedMilliseconds}ms. This handle limit is {HandlerTimeout}ms", 
                    typeof(TRequest).Name,
                    elapsedMilliseconds,
                    handlerTimeout.TimeoutInMilliseconds);
        }
        else
        {
            if (elapsedMilliseconds > configuration.DefaultTimeoutInMilliseconds)
                logger.LogWarning("{Handler} executed in {ElapsedMilliseconds}ms. Default timeout is {DefaultTimeout}ms",
                    typeof(TRequest).Name,
                    elapsedMilliseconds,
                    configuration.DefaultTimeoutInMilliseconds);
        }
        
        return response;
    }
}