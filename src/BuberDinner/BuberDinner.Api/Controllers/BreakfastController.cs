using BuberDinner.Application.Breakfast;
using BuberDinner.Contracts.Breakfast;
using BuberDinner.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("[controller]")]
public class BreakfastController : ApiController
{
    private readonly IBreakfastService breakfastService;

    public BreakfastController(IBreakfastService breakfastService)
    {
        this.breakfastService = breakfastService;
    }

    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            Guid.NewGuid(),
            request.BreakfastDetails.Name,
            request.BreakfastDetails.Description,
            request.BreakfastDetails.StartDateTime,
            request.BreakfastDetails.EndDateTime,
            request.Savory,
            request.Sweet);

        breakfastService.CreateBreakfast(breakfast);

        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: MapBreakfastResponse(breakfast));
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast([FromRoute] Guid id)
    {
        Breakfast? breakfast = breakfastService.GetBreakfast(id);

        if (breakfast is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(breakfast);
    }

    private object? MapBreakfastResponse(Breakfast breakfast)
    {
        throw new NotImplementedException();
    }
}