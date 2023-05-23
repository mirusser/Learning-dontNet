using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.MenuAggregate.ValueObjects;

namespace BuberDinner.Domain.MenuAggregate.Entities;

public sealed class MenuSection : Entity<MenuSectionId>
{
    private readonly List<MenuItem> items = new();
    public string Name { get; private set; }
    public string Description { get; private set; }

    public IReadOnlyList<MenuItem> Items => items.AsReadOnly();

    private MenuSection()
    {
    }

    public MenuSection(
        MenuSectionId id,
        string name,
        string description,
        List<MenuItem>? items) : base(id)
    {
        Name = name;
        Description = description;
        this.items = items;
    }

    public static MenuSection Create(
        string name,
        string description,
        List<MenuItem>? items)
    {
        return new(
            MenuSectionId.CreateUnique(),
            name,
            description,
            items);
    }
}