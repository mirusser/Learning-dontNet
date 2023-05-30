using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Application.UnitTests.Menus.Commands.TestUtils;
using FluentAssertions;
using Moq;
using TestUtils.Menus.Extensions;

namespace BuberDinner.Application.UnitTests.Menus.Commands.CreateMenu;

public class CreateMenuCommandHandlerTests
{
    // naming convention:
    // t1_t2_t3
    // t1: SUT - logical component we're testing
    // t2: scenario - what we're testing
    // t3: expected outcome - what we expect the logical component to do

    private readonly CreateMenuCommandHandler handler;
    private readonly Mock<IMenuRepository> mockMenuRepository;

    public CreateMenuCommandHandlerTests()
    {
        mockMenuRepository = new();
        handler = new(mockMenuRepository.Object);
    }

    [Theory]
    [MemberData(nameof(ValidCreateMenuCommands))]
    public async Task HandleCreateMenuCommand_WhenMenuIsValid_ShouldCreateAndReturnMenu(
        CreateMenuCommand createMenuCommand)
    {
        // act
        var result = await handler.Handle(createMenuCommand);

        // assert
        result.IsError.Should().BeFalse();
        result.Value.ValidateCreatedFrom(createMenuCommand);
        mockMenuRepository.Verify(m => m.AddAsync(result.Value), Times.Once);
    }

    public static IEnumerable<object[]> ValidCreateMenuCommands()
    {
        yield return new[]
{
            CreateMenuCommandUtils.CreateCommand()
        };

        yield return new[]
        {
            CreateMenuCommandUtils.CreateCommand(
                CreateMenuCommandUtils.CreateSectionsCommand(sectionCount: 3))
        };

        yield return new[]
 {
            CreateMenuCommandUtils.CreateCommand(
                CreateMenuCommandUtils.CreateSectionsCommand(
                    sectionCount: 3,
                    CreateMenuCommandUtils.CreateItemsCommand(3)))
        };
    }
}