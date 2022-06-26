using AutoMapper;
using MinimalAPI_Les.Dtos;
using MinimalAPI_Les.Models;

namespace MinimalAPI_Les.Profiles;

public class CommandsProfile : Profile
{
	public CommandsProfile()
	{
		// Source -> Target
		CreateMap<Command, CommandReadDto>();
		CreateMap<CommandCreateDto, Command>();
		CreateMap<CommandUpdateDto, Command>();
	}
}