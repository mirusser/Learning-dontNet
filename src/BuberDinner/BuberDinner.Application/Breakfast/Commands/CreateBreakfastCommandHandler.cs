using BuberDinner.Application.Breakfast.Common;
using BuberDinner.Application.Common.Interfaces.Persistence;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Breakfast.Commands;

public class CreateBreakfastCommandHandler
    : IRequestHandler<CreateBreakfastCommand, ErrorOr<BreafastResult>>
{
    private readonly IBreakfastRepository breakfastRepository;

    public CreateBreakfastCommandHandler(IBreakfastRepository breakfastRepository)
    {
        this.breakfastRepository = breakfastRepository;
    }

    public async Task<ErrorOr<BreafastResult>> Handle(
        CreateBreakfastCommand command,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        Domain.Entities.Breakfast breakfast = new()
        {
            Name = command.Name,
            Description = command.Description,
            StartDateTime = command.StartDateTime,
            EndDateTime = command.EndDateTime,
            Savory = command.Savory,
            Sweet = command.Sweet,
        };

        breakfastRepository.Add(breakfast);

        return new BreafastResult();
    }
}