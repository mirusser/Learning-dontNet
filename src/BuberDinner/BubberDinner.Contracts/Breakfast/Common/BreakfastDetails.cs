namespace BuberDinner.Contracts.Breakfast.Common;

public record BreakfastDetails(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime);