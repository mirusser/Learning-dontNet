using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.Common.ValueObjects;
using BuberDinner.Domain.DinnerAggregate.ValueObjects;
using BuberDinner.Domain.HostAggregate.ValueObjects;
using BuberDinner.Domain.MenuAggregate.Entities;
using BuberDinner.Domain.MenuAggregate.ValueObjects;
using BuberDinner.Domain.MenuReviewAggregate.ValueObjects;

namespace BuberDinner.Domain.MenuAggregate;

public sealed class Menu : AggregateRoot<MenuId>
{
    private readonly List<MenuSection> sections = new();
    private readonly List<DinnerId> dinnerIds = new();
    private readonly List<MenuReviewId> menuReviewIds = new();

    public string Name { get; private set; }
    public string Description { get; private set; }
    public AverageRating AverageRating { get; private set; }
    public IReadOnlyList<MenuSection> Sections => sections.AsReadOnly();

    public HostId HostId { get; private set; }
    public IReadOnlyList<DinnerId> DinnerIds => dinnerIds.AsReadOnly();
    public IReadOnlyList<MenuReviewId> MenuReviewIds => menuReviewIds.AsReadOnly();

    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Menu()
    {
    }

    public Menu(
        MenuId menuId,
        string name,
        string description,
        HostId hostId,
        AverageRating averageRating,
        List<MenuSection> sections,
        DateTime createdDateTime,
        DateTime updatedDateTime) : base(menuId)
    {
        Name = name;
        Description = description;
        HostId = hostId;
        AverageRating = averageRating;
        this.sections = sections;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

    public static Menu Create(
        string name,
        string description,
        string hostId,
        List<MenuSection>? sections)
    {
        return new(
            MenuId.CreateUnique(),
            name,
            description,
            HostId.Create(hostId),
            AverageRating.CreateNew(),
            sections ?? new(),
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}