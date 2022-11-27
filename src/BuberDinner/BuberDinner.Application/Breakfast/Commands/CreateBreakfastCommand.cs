using BuberDinner.Application.Breakfast.Common;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Breakfast.Commands;

public record CreateBreakfastCommand(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    List<string> Savory,
    List<string> Sweet) : IRequest<ErrorOr<BreafastResult>>;