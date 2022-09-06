using Microsoft.AspNetCore.Mvc;
using RedisApi.Data;
using RedisApi.Models;

namespace RedisApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo platformRepo;

    public PlatformsController(IPlatformRepo platformRepo)
    {
        this.platformRepo = platformRepo;
    }

    [HttpGet("{id}", Name = "Get")]
    public ActionResult<Platform> Get(string id)
    {
        var platform = platformRepo.Get(id);

        if (platform is null)
        {
            return NotFound();
        }

        return Ok(platform);
    }

    [HttpPost]
    public ActionResult<Platform> Create(Platform platform)
    {
        platformRepo.Create(platform);

        return CreatedAtRoute(nameof(Get), new { id = platform.Id }, platform);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Platform?>> GetAll()
    {
        var platforms = platformRepo.GetAll();

        return Ok(platforms);
    }
}