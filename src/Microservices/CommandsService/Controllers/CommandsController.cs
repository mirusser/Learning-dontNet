using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit {nameof(GetCommandForPlatform)}, platformId: {platformId}");

            if (!_repo.PlatformExists(platformId)) return NotFound();

            var commands = _repo.GetCommandsForPlatform(platformId);
            var commandsReadDto = _mapper.Map<IEnumerable<CommandReadDto>>(commands);

            return Ok(commandsReadDto);
        }

        [HttpGet("{commandId}", Name = nameof(GetCommandForPlatform))]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit {nameof(GetCommandForPlatform)}, {nameof(platformId)} : {platformId} / {nameof(commandId)} : {commandId}");
            if (!_repo.PlatformExists(platformId)) return NotFound();

            var command = _repo.GetCommand(platformId, commandId);

            if (command is null) return NotFound();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return Ok(commandReadDto);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> Hit {nameof(CreateCommandForPlatform)}, platformId: {platformId}");

            if (!_repo.PlatformExists(platformId)) return NotFound();

            var command = _mapper.Map<Command>(commandCreateDto);

            _repo.CreateCommand(platformId, command);

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(
                nameof(GetCommandForPlatform),
                new { platformId, commandId = command.Id }, commandReadDto);
        }
    }
}