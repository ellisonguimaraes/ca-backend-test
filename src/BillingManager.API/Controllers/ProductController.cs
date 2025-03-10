using System.Text.Json;
using BillingManager.Application.Commands.Products.Create;
using BillingManager.Application.Commands.Products.Update;
using BillingManager.Application.Queries.Products;
using BillingManager.Application.Queries.Products.GetAll;
using BillingManager.Application.Queries.Products.GetById;
using BillingManager.Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HttpResponse = BillingManager.Domain.Utils.HttpResponse;

namespace BillingManager.API.Controllers;

/// <summary>
/// Controller responsible for managing products
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    #region Constants
    private const string X_PAGINATION = "X-Pagination";
    private const string ID = "id";
    #endregion
    
    /// <summary>Get product by identification (Guid)</summary>
    /// <remarks>
    /// Request example:
    /// 
    ///     GET /api/product/{id:guid}
    ///     
    /// </remarks>
    /// <param name="id">Product identifier (Guid)</param>
    /// <returns>Product</returns>
    /// <response code="200">Returns product successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<ProductQueryResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
    public async Task<IActionResult> GetByIdAsync([FromRoute(Name = ID)] Guid id)
    {
        var response = await mediator.Send(new GetProductByIdQuery { Id = id });
        return Ok(new HttpResponse { Data = response });
    }

    /// <summary>Get pagination list of product</summary>
    /// /// <remarks>
    /// This request return response header with pagination infos:
    /// 
    ///     X-Pagination: {"total_count":5,"page_size":3,"page_number":2,"total_pages":2,"has_next":false,"has_previous":true} 
    ///     
    /// </remarks>
    /// <param name="query">Page number and page size</param>
    /// <returns>Paginated products (with header X-Pagination with page info)</returns>
    /// <response code="200">Returns pagination products successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<IList<ProductQueryResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
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
        
        return Ok(new HttpResponse { Data = response.Items });
    }
    
    /// <summary>Create new product</summary>
    /// <remarks>
    /// Request example:
    /// 
    ///     POST /api/product
    ///     {
    ///         "name": "Notebook"
    ///     }
    ///     
    /// </remarks>
    /// <param name="command">Product name, email and address</param>
    /// <returns>Product with identifier (Guid)</returns>
    /// <response code="200">Product created successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<ProductQueryResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(new HttpResponse { Data = response });
    }
    
    /// <summary>Update product</summary>
    /// <remarks>
    /// Request example:
    /// 
    ///     PUT /api/product/{id:guid}
    ///     {
    ///         "name": "Mouse"
    ///     }
    ///     
    /// </remarks>
    /// <param name="command">Product name, email and address</param>
    /// <param name="id">Product identifier (Guid)</param>
    /// <returns>Product updated</returns>
    /// <response code="200">Product updated successfully</response>
    /// <response code="400">Any mapped error occurred</response>
    /// <response code="500">Any unmapped error occurred</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HttpResponse<ProductQueryResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HttpResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(HttpResponse))]
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