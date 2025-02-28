using BillingManager.Domain.Entities;
using BillingManager.Domain.Utils;
using BillingManager.Infra.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BillingManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(IRepository<Product> productRepository) : ControllerBase
{
    private readonly IRepository<Product> productRepository = productRepository;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await productRepository.GetPaginate(new PaginationParameters(1, 4));
        return Ok(products);
    }
}