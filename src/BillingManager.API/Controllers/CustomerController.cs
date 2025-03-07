using MediatR;
using Microsoft.AspNetCore.Mvc;
using BillingManager.Application.Commands.Customers.Create;
using BillingManager.Application.Queries.Customers.GetAll;
using System.Text.Json;
using BillingManager.Application.Commands.Customers.Update;
using BillingManager.Application.Queries.Customers.GetById;

namespace BillingManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(IMediator mediator) : ControllerBase
{
    #region Constants
    private const string X_PAGINATION = "X-Pagination";
    private const string ID = "id";
    #endregion

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute(Name = ID)] Guid id)
    {
        var response = await mediator.Send(new GetCustomerByIdQuery { Id = id });
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginateAsync([FromQuery] GetAllCustomerQuery query)
    {
        var response = await mediator.Send(query);
    
        var metadata = new 
        {
            total_count = response.TotalCount,
            page_size = response.PageSize,
            page_number = response.CurrentPage,
            total_pages = response.TotalPages,
            has_next = response.HasNext,
            has_previous = response.HasPrevious
        };

        Response.Headers.Append(X_PAGINATION, JsonSerializer.Serialize(metadata));

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute(Name = ID)] Guid id, 
        [FromBody] UpdateCustomerCommand command)
    {
        command.Id = id;
        var response = await mediator.Send(command);
        return Ok(response);
    }
}