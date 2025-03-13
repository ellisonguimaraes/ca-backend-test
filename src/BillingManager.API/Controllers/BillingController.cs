using BillingManager.Application.Commands.Billing.ImportBilling;
using BillingManager.Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.API.Controllers;

/// <summary>
/// Controller responsible for managing billing
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BillingController(IMediator mediator) : ControllerBase
{
    /// <summary>Import billing/billinglines from external request</summary>
    /// <remarks>
    /// Request example:
    /// 
    ///     POST /api/billing/import
    ///     
    /// </remarks>
    /// <returns>Logging successful or failed attempts</returns>
    /// <response code="200">Try import billing successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpPost]
    [Route("import")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<IList<ImportBillingCommandResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
    public async Task<IActionResult> ImportBillingAsync()
    {
        var command = await mediator.Send(new ImportBillingCommand());
        return Ok(new HttpResponse { Data = command });
    }
}