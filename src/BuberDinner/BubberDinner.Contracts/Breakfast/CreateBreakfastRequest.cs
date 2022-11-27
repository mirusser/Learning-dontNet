using BuberDinner.Contracts.Breakfast.Common;

namespace BuberDinner.Contracts.Breakfast;

public record CreateBreakfastRequest(
    BreakfastDetails BreakfastDetails,
    List<string> Savory,
    List<string> Sweet);