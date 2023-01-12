using BuberDinner.Domain.HostAggregate.ValueObjects;
using BuberDinner.Domain.MenuAggregate;
using ErrorOr;
using MediatR;
using BuberDinner.Domain.MenuAggregate.Entities;
using BuberDinner.Application.Common.Interfaces.Persistence;

namespace BuberDinner.Application.Menus.Commands.CreateMenu;

public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, ErrorOr<Menu>>
{
    private readonly IMenuRepository menuRepository;

    public CreateMenuCommandHandler(IMenuRepository menuRepository)
    {
        this.menuRepository = menuRepository;
    }

    public async Task<ErrorOr<Menu>> Handle(
        CreateMenuCommand request, 
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // Create Menu
        var menu = Menu.Create(
            request.Name,
            request.Description,
            request.HostId,
            request.Sections
                .ConvertAll(s => MenuSection.Create(
                    s.Name,
                    s.Description,
                    s.Items.ConvertAll(i => MenuItem.Create(i.Name, i.Description)))));

        // Persist Menu
        menuRepository.Add(menu);

        // Return Menu

        return menu;
    }
}