namespace BuberDinner.Contracts.Menus;

public record CreateMenuRequest(
    string Name,
    string Description,
    List<MenuSection> Sections);

public record MenuSection(
    string Name, 
    string Description,
    List<MenutItem> Items);

public record MenutItem(
    string Name,
    string Description);