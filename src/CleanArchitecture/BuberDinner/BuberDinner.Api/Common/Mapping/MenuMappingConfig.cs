using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Contracts.Menu;
using BuberDinner.Domain.MenuAggregate;
using Mapster;
using MenuItem = BuberDinner.Domain.MenuAggregate.Entities.MenuItem;
using MenuSection = BuberDinner.Domain.MenuAggregate.Entities.MenuSection;

namespace BuberDinner.Api.Common.Mapping;

public class MenuMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateMenuRequest Request, string HostId), CreateMenuCommand>()
            .Map(dest => dest.HostId, src => src.HostId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<Menu, MenuResponse>()
            .Map(dest => dest.Id, src => src.GetId())
            .Map(dest => dest.AverageRating,
                src => src.AverageRating.NumRatings > 0
                ? src.AverageRating.Value.ToString()
                : null)
            .Map(dest => dest.HostId, src => src.HostId.Value)
            .Map(dest => dest.DinnerIds,
                src => src.DinnerIds.Select(dinnerId => dinnerId.Value).ToList())
            .Map(dest => dest.MenuReviewIds,
                src => src.MenuReviewIds.Select(menuId => menuId.Value).ToList())
            .Map(dest => dest, src => src);

        config.NewConfig<MenuSection, MenuSectionResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);

        config.NewConfig<MenuItem, MenuItemResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest, src => src);
    }
}