using Microsoft.AspNetCore.Mvc;
using RandomQuotesApi.Clients;
using RandomQuotesApi.Models.DTOs;
using RandomQuotesApi.Repos;
using RandomQuotesApi.Services;

namespace RandomQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuotesController(
    QuoteService quoteService, 
    IQuotableClient quotableClient) : ControllerBase
{
    
    // GET api/quotes/random/external
    [HttpGet("random/external")]
    public async Task<ActionResult<QuoteDto>> GetRandomExternalAsync(CancellationToken ct)
    {
        var result = await quotableClient.GetRandomQuoteAsync(ct);
        
        // TODO: save to db if doesn't exist already
        var dto = new QuoteDto
        {
            Id = Guid.NewGuid(),              
            Text = result.Value.Content,
            Author = result.Value.Author,
            Origin = "Quotable",
            Categories = result.Value.Tags ?? []
        };
        
        return Ok(dto);
    }
    
    // GET api/quotes/random/{userId}
    [HttpGet("random/{userId:guid}")]
    public async Task<ActionResult<QuoteDto>> GetRandom(Guid userId, CancellationToken ct)
    {
        var quote = await quoteService.GetRandomAsync(userId, ct);

        if (quote is null)
            return NotFound(new { message = "No unseen quotes for this user (or no quotes at all)." });

        // Map entity to DTO (don’t leak EF entities directly if you can avoid it)
        var dto = new QuoteDto
        {
            Id = quote.Id,
            Text = quote.Text,
            Author = quote.Author,
            Origin = quote.Origin,
            Categories = quote.Categories
        };

        return Ok(dto);
    }

    // GET api/quotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuoteDto>>> GetAll([FromServices] IQuoteRepository quoteRepo, CancellationToken ct)
    {
        var quotes = await quoteRepo.GetAllAsync(ct);

        var dtos = quotes.Select(q => new QuoteDto
        {
            Id = q.Id,
            Text = q.Text,
            Author = q.Author,
            Origin = q.Origin,
            Categories = q.Categories
        });

        return Ok(dtos);
    }

    [HttpGet("clear/{userId:guid}")]
    public async Task<ActionResult<string>> ClearSeen(Guid userId, CancellationToken ct)
    {
        await quoteService.ClearSeenAsync(userId, ct);

        return Ok();
    }
    
    // GET api/quotes/paged?pageNumber=1&pageSize=10
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<QuoteDto>>> GetPaged(
        [FromServices] IQuoteRepository quoteRepo,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest(new
            {
                message = "pageNumber and pageSize must be positive integers."
            });
        }

        var (quotes, totalCount) = await quoteRepo.GetPageAsync(pageNumber, pageSize, ct);

        var dtos = quotes.Select(q => new QuoteDto
        {
            Id = q.Id,
            Text = q.Text,
            Author = q.Author,
            Origin = q.Origin,
            Categories = q.Categories
        }).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var result = new PagedResult<QuoteDto>(
            Items: dtos,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        );

        return Ok(result);
    }
}

public class QuoteDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string[] Categories { get; set; } = [];
}