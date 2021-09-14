using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;

namespace PlatformService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public PlatformController(IPlatformRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("Getting platforms...");
            var platforms = _repo.GetAtllPlatforms();
            var platformReadDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return Ok(platformReadDtos);
        }

        [HttpGet("{id}", Name = nameof(GetPlatformById))]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _repo.GetPlatformById(id);
            if (platform is not null)
            {
                var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
                return Ok(platformReadDto);
            }

            return NotFound();
        }
    }
}