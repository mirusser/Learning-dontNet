
using Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Multitenant.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(int id)
    {
        var productDetails = await service.GetByIdAsync(id);
        
        return Ok(productDetails);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProductRequest request)
    {
        return Ok(await service.CreateAsync(request.Name, request.Description, request.Rate));
    }
}

public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Rate { get; set; }
}