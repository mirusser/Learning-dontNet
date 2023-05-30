using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Application.UnitTests.TestUtils.Constants;

namespace BuberDinner.Application.UnitTests.Menus.Commands.TestUtils;

public static class CreateMenuCommandUtils
{
    public static CreateMenuCommand CreateCommand(
        List<CreateMenuSectionCommand>? sections = null)
        => new(
            Constants.Host.Id.Value.ToString(),
            Constants.Menu.Name,
            Constants.Menu.Description,
            sections ?? CreateSectionsCommand())
        {
        };

    public static List<CreateMenuSectionCommand> CreateSectionsCommand(
        int sectionCount = 1,
        List<CreateMenuItemCommand>? items = null)
        => Enumerable.Range(0, sectionCount)
        .Select(index => new CreateMenuSectionCommand(
            Constants.Menu.SectionNameFromIndex(index),
            Constants.Menu.SectionDescriptionFromIndex(index),
            items ?? CreateItemsCommand(index)))
        .ToList();

    public static List<CreateMenuItemCommand> CreateItemsCommand(int itemCount = 1)
        => Enumerable.Range(0, itemCount)
        .Select(index => new CreateMenuItemCommand(
            Constants.Menu.ItemNameFromIndex(index),
            Constants.Menu.ItemDescriptionFromIndex(index)))
        .ToList();
}