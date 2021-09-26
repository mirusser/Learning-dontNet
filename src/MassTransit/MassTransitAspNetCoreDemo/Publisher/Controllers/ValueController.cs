using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Publisher.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ValueController : ControllerBase
{
    private readonly ILogger<ValueController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public ValueController(
        ILogger<ValueController> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Post(string value)
    {
        _logger.LogInformation("Started sending/publishing a message...");

        await _publishEndpoint.Publish<ValueEntered>(new { Value = value });

        return Ok();
    }
}
