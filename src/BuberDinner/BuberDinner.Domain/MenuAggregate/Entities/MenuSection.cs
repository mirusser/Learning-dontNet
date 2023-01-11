using BuberDinner.Domain.MenuAggregate.ValueObjects;
using BuberDinner.Domain.Models;

namespace BuberDinner.Domain.MenuAggregate.Entities;

public sealed class MenuSection : Entity<MenuSectionId>
{
    private readonly List<MenuItem> items = new();
    public string Name { get; }
    public string Description { get; }
    public IReadOnlyList<MenuItem> Items => items.AsReadOnly();

    public MenuSection(
        MenuSectionId id,
        string name,
        string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public static MenuSection Create(
        string name,
        string description)
    {
        return new(
            MenuSectionId.CreteUnique(),
            name,
            description);
    }
}