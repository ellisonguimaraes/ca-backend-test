using MediatR;
using Microsoft.AspNetCore.Mvc;
using BillingManager.Application.Commands.Customers.Create;
using BillingManager.Application.Queries.Customers.GetAll;
using System.Text.Json;
using BillingManager.Application.Commands.Customers.Update;
using BillingManager.Application.Queries.Customers;
using BillingManager.Application.Queries.Customers.GetById;
using BillingManager.Domain.Utils;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.API.Controllers;

/// <summary>
/// Controller responsible for managing customers
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomerController(IMediator mediator) : ControllerBase
{
    #region Constants
    private const string X_PAGINATION = "X-Pagination";
    private const string ID = "id";
    #endregion
    
    /// <summary>Get customer by identification (Guid)</summary>
    /// <remarks>
    /// Request example:
    /// 
    ///     GET /api/customer/{id:guid}
    ///     
    /// </remarks>
    /// <param name="id">Customer identifier (Guid)</param>
    /// <returns>Customer</returns>
    /// <response code="200">Returns customer successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<CustomerQueryResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
    public async Task<IActionResult> GetByIdAsync([FromRoute(Name = ID)] Guid id)
    {
        var response = await mediator.Send(new GetCustomerByIdQuery { Id = id });
        return Ok(new HttpResponse { Data = response });
    }
    
    /// <summary>Get pagination list of customer</summary>
    /// <remarks>
    /// This request return response header with pagination infos:
    /// 
    ///     X-Pagination: {"total_count":5,"page_size":3,"page_number":2,"total_pages":2,"has_next":false,"has_previous":true} 
    ///     
    /// </remarks>
    /// <param name="query">Page number and page size</param>
    /// <returns>Paginated customers (with header X-Pagination with page info)</returns>
    /// <response code="200">Returns pagination customers successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<IList<CustomerQueryResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
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

        return Ok(new HttpResponse { Data = response.Items });
    }
    
    /// <summary>Create new customer</summary>
    /// <remarks>
    /// Request example:
    /// 
    ///     POST /api/customer
    ///     {
    ///         "name": "Antônio",
    ///         "email": "antonio@gmail.com",
    ///         "address": "rua F"
    ///     }
    ///     
    /// </remarks>
    /// <param name="command">Customer name, email and address</param>
    /// <returns>Customer with identifier (Guid)</returns>
    /// <response code="200">Customer created successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<CustomerQueryResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(new HttpResponse { Data = response });
    }
    
    /// <summary>Update customer</summary>
    /// <remarks>
    /// Request example:
    /// 
    ///     PUT /api/customer/{id:guid}
    ///     {
    ///         "name": "Antônio",
    ///         "email": "antonio@gmail.com",
    ///         "address": "rua F"
    ///     }
    ///     
    /// </remarks>
    /// <param name="command">Customer name, email and address</param>
    /// <param name="id">Customer identifier (Guid)</param>
    /// <returns>Customer updated</returns>
    /// <response code="200">Customer updated successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpPut]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<CustomerQueryResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute(Name = ID)] Guid id, 
        [FromBody] UpdateCustomerCommand command)
    {
        command.Id = id;
        var response = await mediator.Send(command);
        return Ok(new HttpResponse { Data = response });
    }
}