using System.Text.Json;
using BillingManager.Application.Commands.Products.Create;
using BillingManager.Application.Commands.Products.Update;
using BillingManager.Application.Queries.Products.GetAll;
using BillingManager.Application.Queries.Products.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    #region Constants
    private const string X_PAGINATION = "X-Pagination";
    private const string ID = "id";
    #endregion

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute(Name = ID)] Guid id)
    {
        var response = await mediator.Send(new GetProductByIdQuery { Id = id });
        return Ok(new HttpResponse { Data = response });
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginateAsync([FromQuery] GetAllProductQuery query)
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
        
        return Ok(new HttpResponse { Data = response });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(new HttpResponse { Data = response });
    }
    
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute(Name = ID)] Guid id, 
        [FromBody] UpdateProductCommand command)
    {
        command.Id = id;
        var response = await mediator.Send(command);
        return Ok(new HttpResponse { Data = response });
    }
}