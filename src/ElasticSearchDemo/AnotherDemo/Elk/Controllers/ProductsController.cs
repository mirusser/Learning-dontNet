using Elk.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Elk.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IElasticClient elasticClient;
    private readonly ILogger<ProductsController> logger;

    public ProductsController(
        IElasticClient elasticClient,
        ILogger<ProductsController> logger)
    {
        this.elasticClient = elasticClient;
        this.logger = logger;
    }

    [HttpGet(Name = "GetProducts")]
    public async Task<IActionResult> Get(string keyword)
    {
        var results = await elasticClient.SearchAsync<Product>(
            s => s.Query(
                q => q.QueryString(
                    d => d.Query($"*{keyword}*")))
            .Size(1000));

        return Ok(results.Documents.ToList());
    }

    [HttpPost(Name = "AddProducts")]
    public async Task<IActionResult> Post(Product product)
    {
        var response = await elasticClient.IndexDocumentAsync(product);

        return Ok(response.Result);
    }
}